using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Data.Model
{
    public class CmsReportItem : IDigitalDataModel
    {
        public string Title { get; set; }

        public Guid Id { get; set; }

        public string UrlName { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModified { get; set; }

        public WorkflowStatus Status { get; set; }
    }
}
