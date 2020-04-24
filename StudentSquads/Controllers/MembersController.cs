﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentSquads.Models;
using StudentSquads.ViewModels;

namespace StudentSquads.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        private ApplicationDbContext _context;
        public MembersController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //Вот эту функцию можно будет удалить/(это для проверки ролей)
        public ViewResult Index()
        {
            var members = _context.Members.Include(m => m.Squad).ToList();
            if (User.IsInRole("SquadManager"))
                return View("SquadManager",members);
            else
                return View("ShowAll", members);
        }
        public ActionResult ShowAll(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Squad";
            var members = _context.Members.Include(m => m.Squad).Include(m => m.Status).ToList();
            return View(members);

            //return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }
        [HttpPost]
        public ActionResult ApplyForEnter(PersonMainFormViewModel model)
        {
            //var personInDb = _context.People.SingleOrDefault(p => p.Id == id);
            //if (personInDb == null) return RedirectToAction("PersonMainForm", "People");
            Member newMember = new Member
            {
                Id = Guid.NewGuid(),
                PersonId = model.Person.Id,
                SquadId = model.SquadId
            };
            _context.Members.Add(newMember);
            _context.SaveChanges();
            return RedirectToAction("PersonMainForm","People");
        }
        public ActionResult EnterApplications()
        {
            List<ApplicationsListViewModel> listmodel = new List<ApplicationsListViewModel>();
            string id = User.Identity.GetUserId();
            var person = _context.People.SingleOrDefault(u => u.ApplicationUserId == id);
            //Проверяем 2 условия. В таблице "Руководителей" личность совпадает с текущей, а также дата окончания должности не равна null
            var headofsquad = _context.HeadsOfStudentSquads.SingleOrDefault(h => (h.PersonId == person.Id) && (h.DateofEnd == null));
            ////Но вообще у пользователя, который не является руководителем, даже шанса не должно быть использовать этот запрос
            if (headofsquad == null) RedirectToAction("PersonMainForm", "People");
            dynamic model = new ExpandoObject();
            List<ExpandoObject> joinData = new List<ExpandoObject>();
            if (User.IsInRole("SquadManager"))
            {
                //Выбираем без даты вступления, и решение ком. состава либо true, либо null
                var members = _context.Members.Include(m => m.Person).Include(m => m.Squad)
                    .Where(m => (m.DateOfEnter == null) && (m.ApprovedByCommandStaff != false)).ToList();
                //var query = from m in _context.Members
                //            join f in _context.FeePayments
                //            on m.PersonId equals f.PersonId into list
                //            from x in list.DefaultIfEmpty()
                //            select new
                //            {
                //                m.PersonId,
                //                m.SquadId,
                //                Payment = x?.Id ?? Guid.Empty
                //              };

                foreach (var member in members)
                {
                    string status = "";
                    if (member.ApprovedByCommandStaff == null) status = "Не рассмотрено";
                    else status = "На рассмотрении рег. штабом";
                    ApplicationsListViewModel newapplication = new ApplicationsListViewModel
                    {
                        Member = member,
                        FeePayments = _context.FeePayments.Where(m => m.PersonId == member.Person.Id).ToList(),
                        Status = status
                    };
                    listmodel.Add(newapplication);
                }
            }
            if (User.IsInRole("RegionalManager"))
            {
                ////Проверяем 2 условия. В таблице "Руководителей" личность совпадает с текущей, а также дата окончания должности не равна null
                //var headofsquad = _context.HeadsOfStudentSquads.SingleOrDefault(h => (h.PersonId == person.Id) && (h.DateofEnd == null));
                //////Но вообще у пользователя, который не является руководителем, даже шанса не должно быть использовать этот запрос
                //if (headofsquad == null) RedirectToAction("PersonMainForm", "People");
            }
            return View(listmodel);
        }
    }
}