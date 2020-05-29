using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class ServiceStatuesModel
    {
        public DateTime CheckDateTime { get; set; }

        public Collection<ServiceStatusModel> ServiceStatues { get; set; }
    }
}