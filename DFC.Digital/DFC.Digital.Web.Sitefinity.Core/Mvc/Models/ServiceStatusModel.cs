using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class ServiceStatusModel
    {
        public string Name { get; set; }

        public ServiceState Status { get; set; }

        public string StatusText { get; set; }

        public string CheckCorrelationId { get; set; }
    }
}