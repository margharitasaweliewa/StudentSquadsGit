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
namespace StudentSquads.Controllers.API
{
    public class WorksAuditController : ApiController
    {
        private ApplicationDbContext _context;
        public WorksAuditController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [HttpPost]
        public IHttpActionResult ApproveChange(string id)
        {
            var workInDb = _context.Works.Single(w => w.Id.ToString() == id);
            //Утверждаем изменения
            workInDb.Approved = true;
            //Если удаление, проставлеяем метку "Удалено"
            if (workInDb.WorkChangeTypeId == 2)
            {
                //Сделать удаленным записи, у которых OriginalWork = текущий OriginalWork
                var works = _context.Works.Where(w => w.OriginalWorkId == workInDb.OriginalWorkId);
                foreach (var work in works)
                {
                    work.Removed = DateTime.Now;
                }
            }
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult RejectChange(string id)
        {
            var workInDb = _context.Works.Single(w => w.Id.ToString() == id);
            //Отклоняем изменения
            workInDb.Approved = false;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult Rebuke(WorkViewModel model) 
        {
            foreach (var work in model.ChoosenWorks)
            {
                //Берем все записи аудита
                var worksInDb = _context.Works.Where(w => w.OriginalWorkId == work);
                //Для каждой записи проставляем незачет целины и причину выговора
                foreach (var workInDb in worksInDb)
                {
                    workInDb.Rebuke = model.Rebuke;
                    workInDb.Affirmed = false;
                }
            }
            return Ok();
        }
    }
}
