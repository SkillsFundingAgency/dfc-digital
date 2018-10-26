using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class ApprenticeshipVacancyReport : IDigitalDataModel
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public SocCode SocCode { get; set; }
    }
}
