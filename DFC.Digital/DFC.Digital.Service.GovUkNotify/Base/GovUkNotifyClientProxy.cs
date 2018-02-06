using DFC.Digital.Core.Utilities;
using Notify.Client;
using Notify.Models.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.GovUkNotify.Base
{
    internal class GovUkNotifyClientProxy : IGovUkNotifyClientProxy
    {
        /// <summary>
        /// This is only a proxy to GovNotify client as there are no interface we could hang our unit tests over
        /// </summary>
        /// <param name="apiKey">The key needed for gov uk notify api</param>
        /// <param name="emailAddress">email address to send email</param>
        /// <param name="templateId">template id</param>
        /// <param name="personalisation">dictionary of personalisation</param>
        /// <returns><see cref="EmailNotificationResponse"/></returns>
        public EmailNotificationResponse SendEmail(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> personalisation)
        {
            var client = new NotificationClient(apiKey);
            return client.SendEmail(emailAddress, templateId, personalisation);
        }
    }
}
