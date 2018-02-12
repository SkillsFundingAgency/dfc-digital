using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class ServiceStatusModel
    {
        public DateTime CheckDateTime { get; set; }

        public IEnumerable<ServiceStatus> ServiceStatues { get; set; }
    }
}