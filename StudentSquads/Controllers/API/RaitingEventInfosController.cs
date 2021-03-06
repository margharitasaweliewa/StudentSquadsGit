﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using StudentSquads.Models;
using StudentSquads.ViewModels;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Dynamic;
using System.ComponentModel;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentSquads.Controllers;
using System.Web.Services.Protocols;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;

namespace StudentSquads.Controllers.API
{
    public class RaitingEventInfosController : ApiController
    {
        private ApplicationDbContext _context;
        public RaitingEventInfosController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [HttpGet]
        public List<RaitingEventInfoViewModel> AllRaitingEventInfos()
        {
            List <RaitingEventInfoViewModel> listinfos = new List<RaitingEventInfoViewModel>();
            //Находим текущий рейтинг
            var raitings = _context.Raitings.Where(r => r.DateofCreation == null).ToList();
            //Если нашелся текущий рейтинг
            if (raitings.Count() == 1) 
            {
                var raiting = raitings[0];
                //Берем только текущий рейтинг
                var infos = _context.RaitingEventInfos.Include(r => r.RaitingEvent).Include(r => r.RaitingSection).Include(r => r.Squad)
                    .Where(r => r.RaitingEvent.RaitingId == raiting.Id)
                   .ToList();
                foreach (var info in infos)
                {
                    string status = "Нет решения";
                    if (info.Approved == true) status = "Принято";
                    else if(info.Approved == false) status = "Отклонено";
                    RaitingEventInfoViewModel newinfo = new RaitingEventInfoViewModel
                    {
                        Id = info.Id,
                        Squad = info.Squad.Name,
                        Uni = _context.Squads.Include(s => s.UniversityHeadquarter)
                        .Single(s => s.Id == info.SquadId).UniversityHeadquarter.University,
                        MembershipCount = info.MembershipCount.ToString(),
                        CreateDate = info.CreateTime.ToString("dd.MM.yyyy"),
                        FilesCount = _context.RaitingEventInfoFiles
                        .Where(r => r.RaitingEventInfoId == info.Id).ToList().Count(),
                        Event = info.RaitingEvent.Name,
                        Status = status,
                        MembershipType = _context.RaitingSections.Include(m => m.MembershipType)
                        .Single(m => m.Id == info.RaitingSectionId).MembershipType.Name
                    };
                    listinfos.Add(newinfo);
                }
            }
            return listinfos;
        }
        [HttpPost]
        public IHttpActionResult NewInfo(RaitingEventInfoViewModel model)
        {
            if (User.IsInRole("SquadManager"))
            {
                //Находим текущего пользователя
                string id = User.Identity.GetUserId();
                var person = _context.People.SingleOrDefault(u => u.ApplicationUserId == id);
                //Проверяем 2 условия. В таблице "Руководителей" личность совпадает с текущей, а также должность активна
                var headofsquad = _context.HeadsOfStudentSquads.Include(h => h.MainPosition).Include(h => h.Squad)
                    .Include(h => h.UniversityHeadquarter).Include(h => h.RegionalHeadquarter)
                    .SingleOrDefault(h => (h.PersonId == person.Id) && (h.DateofEnd == null) && (h.DateofBegin != null));
                //Если активной записи о руководстве не найдено, перенаправляем на главную страницу
                int count;
                //Пытаемся конвертировать int
                try { count = Convert.ToInt32(model.MembershipCount); }
                catch (Exception) { return BadRequest(); }
                RaitingEventInfo newinfo = new RaitingEventInfo();
                //Если новая запись
                if (model.Id == Guid.Empty)
                {
                    newinfo.CreateTime = DateTime.Now;
                    newinfo.SquadId = headofsquad.SquadId;
                }
                //Если запись редактируется
                else newinfo = _context.RaitingEventInfos.Single(n => n.Id ==model.Id);
                newinfo.MembershipCount = count;
                //Находим показатель, к которому относится
                RaitingSection thissection = new RaitingSection();
                if (model.EventId != Guid.Empty) 
                { newinfo.RaitingEventId = model.EventId;
                    //Какого уровня текущее мероприятие
                    var level = _context.RaitingEvents.Include(e => e.EventLevel)
                        .Single(e => e.Id == model.EventId);
                   //Тип участия
                    int membershipTypeId = Convert.ToInt32(model.MembershipTypeId);
                    //Находим все показатели с таким типом участия, которые не были удалены
                    var raitingsections = _context.RaitingSections.Include(r => r.MembershipType)
                        .Where(r => (r.MembershipTypeId == membershipTypeId)&&(r.Removed!=true)).ToList();
                    foreach (var section in raitingsections)
                    {
                        //Уровени
                        var levels = _context.RaitingSectionLevels
                            .Where(l => l.RaitingSectionId == section.Id).Select(l => l.EventLevelId).ToList();
                        if (levels.Contains(level.EventLevelId))
                        {
                            thissection = section;
                            break;
                        }
                    }
                    if (thissection.Id == Guid.Empty) return BadRequest();
                    newinfo.RaitingSectionId = thissection.Id;
                }
                //Если новая запись, добавляем ей Id и добавляем запись в базу
                if (newinfo.Id==Guid.Empty) 
                {
                    newinfo.Id = Guid.NewGuid();
                    _context.RaitingEventInfos.Add(newinfo);
                }
                //Если ссылки есть, добавляем ссылки
                if (model.ReferenceDescriptions != null) 
                { 
                foreach(var refer in model.ReferenceDescriptions)
                {
                    RaitingEventInfoFile newref = new RaitingEventInfoFile
                    {
                        Id = Guid.NewGuid(),
                        Path = refer.Key,
                        Description = refer.Value,
                        RaitingEventInfoId = newinfo.Id
                    };
                    _context.RaitingEventInfoFiles.Add(newref);
                }
                }
                _context.SaveChanges();
                return Ok();
            }
            else return BadRequest();

        }
        [HttpPut]
        public IHttpActionResult ApproveInfo(Guid id)
        {
            var info = _context.RaitingEventInfos.Single(r => r.Id == id);
            info.Approved = true;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult RejectInfo(Guid id)
        {
            var info = _context.RaitingEventInfos.Single(r => r.Id == id);
            info.Approved = false;
            _context.SaveChanges();
            return Ok();
        }
    }
}
