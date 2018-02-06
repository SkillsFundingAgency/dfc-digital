using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Data.Model
{
    public class PreSearchFilter : IDigitalDataModel
    {
        public Guid? Id { get; set; }

        public string UrlName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Order { get; set; }

        public bool? NotApplicable { get; set; }
    }
}
