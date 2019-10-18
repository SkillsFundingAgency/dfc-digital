using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class SocCode : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string SOCCode { get; set; }

        public string Description { get; set; }

        public string ONetOccupationalCode { get; set; }

        public string UrlName { get; set; }

        public IEnumerable<Classification> ApprenticeshipFramework { get; set; }

        public IEnumerable<Classification> ApprenticeshipStandards { get; set; }
    }
}
