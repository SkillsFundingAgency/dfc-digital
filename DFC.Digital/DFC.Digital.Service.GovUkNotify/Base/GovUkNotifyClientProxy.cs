using Notify.Client;
using Notify.Models.Responses;
using System.Collections.Generic;

namespace DFC.Digital.Service.GovUkNotify.Base
{
    public class GovUkNotifyClientProxy : IGovUkNotifyClientProxy
    {
        /// <summary>
        /// This is only a proxy to GovNotify client as there are no interface we could hang our unit tests over
        /// </summary>
        /// <param name="apiKey">The key needed for gov uk notify api</param>
        /// <param name="emailAddress">email address to send email</param>
        /// <param name="templateId">template id</param>
        /// <param name="notifyUkDynamicObject">dictionary of personalisation</param>
        /// <returns><see cref="EmailNotificationResponse"/></returns>
        public EmailNotificationResponse SendEmail(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> notifyUkDynamicObject)
        {
            var client = new NotificationClient(apiKey);
            return client.SendEmail(emailAddress, templateId, notifyUkDynamicObject);
        }
    }
}