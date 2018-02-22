using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class ServiceStatusModel
    {
        public DateTime CheckDateTime { get; set; }

        public List<ServiceStatus> ServiceStatues { get; set; }
    }
}