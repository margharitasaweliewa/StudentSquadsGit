﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentSquads.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string PatronymicName { get; set; }
        //true - мужской, false-женский
        public bool Sex { get; set; }
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateofBirth { get; set; }
        [Display(Name = "ИНН")]
        public string INN { get; set; }
        [Display(Name = "СНИЛС")]
        public string Snils { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        [Display(Name = "Серия паспорта")]
        public string PasportSerie { get; set; }
        [Display(Name = "Номер паспорта")]
        public string PassportNumber{ get; set; }
        [Display(Name = "Дата выдачи паспорта")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateofIssue  { get; set; }
        [Display(Name = "Код подразделения")]
        public string DepartmentCode { get; set; }
        [Display(Name = "Город рождения")]
        public string CityofBirth { get; set; }
        [Display(Name = "Место регистрации (по паспорту)")]
        public  string RegistrationPlace { get; set; }
        [Display(Name = "Дата вступления")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfEnter{ get; set; }
        [Display(Name = "Дата исключения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfExit { get; set; }
        [Display(Name = "Номер членского билета")]
        public string MembershipNumber { get; set; }
        //null - зарегистрирован в системе,
        //false - из группы VK, true - сайт "Студотряды.рф"
        public bool? ApplicationFrom { get; set; }


    }
}