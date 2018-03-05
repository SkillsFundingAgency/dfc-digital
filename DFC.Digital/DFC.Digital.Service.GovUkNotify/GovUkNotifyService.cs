using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.GovUkNotify.Base;
using Notify.Exceptions;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.GovUkNotify
{
    /// <summary>
    /// Gov Uk Notify Implementation , it implements the base class function to perform its conversion or initialize its configuration
    /// </summary>
    /// <seealso cref="DFC.Digital.Data.Interfaces.IGovUkNotify" />
    /// <seealso />
    public class GovUkNotifyService : IGovUkNotify
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IGovUkNotifyClientProxy clientProxy;

        public GovUkNotifyService(IApplicationLogger applicationLogger, IGovUkNotifyClientProxy clientProxy)
        {
            this.applicationLogger = applicationLogger;
            this.clientProxy = clientProxy;
        }

        /// <summary>
        /// Submits the email.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="vocPersonalisation">Stores dictionary of jpprofile and clientid</param>
        /// <returns>true or false</returns>
        public bool SubmitEmail(string emailAddress, VocSurveyPersonalisation vocPersonalisation)
        {
            try
            {
                var templateId = ConfigurationManager.AppSettings[Constants.GovUkNotifyTemplateId];
                var apiKey = ConfigurationManager.AppSettings[Constants.GovUkNotifyApiKey];
                var response = clientProxy.SendEmail(apiKey, emailAddress, templateId, this.Convert(vocPersonalisation));
                return !string.IsNullOrEmpty(response?.id);
            }
            catch (NotifyClientException ex)
            {
                applicationLogger.ErrorJustLogIt("Failed to send VOC email", ex);
                return false;
            }
        }

        public Dictionary<string, dynamic> Convert(VocSurveyPersonalisation vocSurveyPersonalisation)
        {
            foreach (var item in vocSurveyPersonalisation.Personalisation.ToArray())
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    vocSurveyPersonalisation.Personalisation[item.Key] = Constants.Unknown;
                }
            }

            return vocSurveyPersonalisation.Personalisation
                .ToDictionary<KeyValuePair<string, string>, string, dynamic>(vocObj => vocObj.Key, vocObj => vocObj.Value);
        }
    }
}