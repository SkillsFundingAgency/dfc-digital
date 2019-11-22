using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class RelatedSocCodeItem : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string SOCCode { get; set; }
    }
}
