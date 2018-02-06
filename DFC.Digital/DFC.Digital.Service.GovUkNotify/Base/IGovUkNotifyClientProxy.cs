using Notify.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.GovUkNotify.Base
{
    public interface IGovUkNotifyClientProxy
    {
        EmailNotificationResponse SendEmail(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> notifyUkDynamicObject);
    }
}
