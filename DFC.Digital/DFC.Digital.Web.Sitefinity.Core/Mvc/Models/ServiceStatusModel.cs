using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class ServiceStatusModel
    {
        public DateTime CheckDateTime { get; set; }

        public Collection<ServiceStatus> ServiceStatues { get; set; }
    }
}