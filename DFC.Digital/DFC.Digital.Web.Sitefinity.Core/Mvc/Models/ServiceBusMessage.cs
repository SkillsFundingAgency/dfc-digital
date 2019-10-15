using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class ServiceBusMessage : Message
    {
        public ServiceBusMessage(byte[] body) : base(body)
        {
        }

        public string ItemId { get; set; }

        public string JPParentId { get; set; }

        public string CanonicalUrl { get; set; }

        public string MetaTags { get; set; }

        public string EventType { get; set; }

        public string CType { get; set; }

        public DateTime LastPublished { get; set; }
    }
}