﻿using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class ProfileAndApprenticeshipReport : IDigitalDataModel
    {
        public JobProfileReport JobProfile { get; set; }

        public SocCode SocCode { get; set; }

        public IQueryable<string> Frameworks { get; set; }

        public IQueryable<string> Standards { get; set; }

        public IList<ApprenticeshipVacancyReport> ApprenticeVacancies { get; set; }
    }
}
