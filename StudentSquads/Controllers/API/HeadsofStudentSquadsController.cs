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

namespace StudentSquads.Controllers.API
{
    public class HeadsofStudentSquadsController : ApiController
    {
        private ApplicationDbContext _context;
        public HeadsofStudentSquadsController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [HttpGet]
        public List<DesignationViewModel> GetHeads()
        {
            List<DesignationViewModel> listofheads = new List<DesignationViewModel>();
            var heads = _context.HeadsOfStudentSquads.Include(h => h.Person).Include(h => h.MainPosition).Include(h => h.Squad)
                .Include(h => h.UniversityHeadquarter)
                .Where(h => (h.MainPositionId != null))
                .OrderBy(p => p.DateofEnd).ThenByDescending(p => p.DateofBegin)
                .ToList();
            //Находим текущего пользователя
            string id = User.Identity.GetUserId();
            var person = _context.People.SingleOrDefault(u => u.ApplicationUserId == id);
            //Проверяем 2 условия. В таблице "Руководителей" личность совпадает с текущей, а также должность активна
            var headofsquad = _context.HeadsOfStudentSquads.Include(h => h.MainPosition).Include(h => h.Squad)
                .Include(h => h.UniversityHeadquarter).Include(h => h.RegionalHeadquarter).ToList()
                .SingleOrDefault(h => (h.PersonId == person.Id) && (h.DateofEnd == null) && (h.DateofBegin != null));
            //Сокращаем по штабу, если руководитель штаба
            if (headofsquad.UniversityHeadquarterId != null)
            {
                heads = heads.Where(h => h.SquadId != null).ToList();
                heads = heads.Where(h => h.Squad.UniversityHeadquarterId == headofsquad.UniversityHeadquarterId).ToList();
            }
            //Убираем руководителей рег.отделения, если работники рег. отделения
            else if(headofsquad.RegionalHeadquarterId!= null)
            {
                heads = heads.Where(h => h.RegionalHeadquarterId==null)
                    .ToList();
            }
            foreach (var head in heads)
            {
                string place = "";
                if (head.SquadId != null) place = head.Squad.Name;
                else if (head.UniversityHeadquarterId != null) place = head.UniversityHeadquarter.University;
                   DesignationViewModel newModel = new DesignationViewModel
                    {
                        PersonId = head.PersonId,
                        FIO = head.Person.FIO,
                        Position = head.MainPosition.Name,
                        DateofBegin = head.DateofBegin?.ToString("dd.MM.yyyy"),
                        DateofEnd = head.DateofEnd?.ToString("dd.MM.yyyy"),
                        Place = place
                    };
                    listofheads.Add(newModel); 
            }
            return listofheads;
        }
        [HttpPost]
        public IHttpActionResult ApplyForManagersChange(DesignationViewModel head)
        {
            //Находим текущего пользователя
            string id = User.Identity.GetUserId();
            var person = _context.People.SingleOrDefault(u => u.ApplicationUserId == id);
            //Проверяем 2 условия. В таблице "Руководителей" личность совпадает с текущей, а также должность активна
            var headofsquad = _context.HeadsOfStudentSquads.Include(h => h.MainPosition).Include(h => h.Squad)
                .Include(h => h.UniversityHeadquarter).Include(h => h.RegionalHeadquarter)
                .SingleOrDefault(h => (h.PersonId == person.Id) && (h.DateofEnd == null) && (h.DateofBegin != null));
            ////Находим, кто ноходится в настоящее время на должности
            //var manager = _context.HeadsOfStudentSquads
            //    .SingleOrDefault(m => (m.MainPositionId.ToString()==head.MainPosition)&&(m.DateofBegin!=null)&&(m.DateofEnd==null)&&(m.SquadId ==headofsquad.SquadId));
            //if(manager)
            int main = Convert.ToInt32(head.MainPosition);
            var mainposition = _context.MainPositions.Single(m => m.Id == main);
            HeadsOfStudentSquads newhead = new HeadsOfStudentSquads
            {
                Id = Guid.NewGuid(),
                PersonId = head.PersonId,
                MainPositionId = main,
                SquadId = headofsquad.SquadId,
                HasRole = true,
                Position = mainposition.Name + " " + headofsquad.Squad.Name
            };
            _context.HeadsOfStudentSquads.Add(newhead);
            _context.SaveChanges();
            return Ok();
        }
    }
}
