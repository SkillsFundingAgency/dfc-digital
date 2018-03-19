using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class ServiceStatusModel
    {
        public DateTime CheckDateTime { get; set; }

        public Collection<ServiceStatus> ServiceStatues { get; set; }
    }
}