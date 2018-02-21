using Notify.Models.Responses;
using System.Collections.Generic;

namespace DFC.Digital.Service.GovUkNotify.Base
{
    public interface IGovUkNotifyClientProxy
    {
        EmailNotificationResponse SendEmail(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> notifyUkDynamicObject);
    }
}
