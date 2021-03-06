﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentSquads.Models
{
    public class RaitingEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EventLevel EventLevel { get; set; }
        public int EventLevelId { get; set; }
        public Squad Squad { get; set; }
        public Guid? SquadId { get; set; }
        public UniversityHeadquarter UniversityHeadquarter { get; set; }
        public Guid? UniversityHeadquarterId { get; set; }
        public RegionalHeadquarter RegionalHeadquarter { get; set; }
        public Guid? RegionalHeadquarterId { get; set; }
        public Raiting Raiting { get; set; }
        public Guid RaitingId { get; set; }
        public bool? Approved { get; set; }
        public DateTime DateofBegin { get; set; }
        public DateTime DateofEnd { get; set; }
        public string DocumentPath { get; set; }
    }
}