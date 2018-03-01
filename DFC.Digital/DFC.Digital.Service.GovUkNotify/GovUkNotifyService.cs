using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.GovUkNotify.Base;
using Notify.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.GovUkNotify
{
    /// <summary>
    /// Gov Uk Notify Implementation , it implements the base class function to perform its conversion or initialize its configuration
    /// </summary>
    /// <seealso cref="DFC.Digital.Data.Interfaces.IGovUkNotify" />
    /// <seealso />
    public class GovUkNotifyService : IGovUkNotify, IServiceStatus
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IGovUkNotifyClientProxy clientProxy;

        public GovUkNotifyService(IApplicationLogger applicationLogger, IGovUkNotifyClientProxy clientProxy)
        {
            this.applicationLogger = applicationLogger;
            this.clientProxy = clientProxy;
        }

        #region Implement of IServiceStatus
        private string ServiceName => "Notification Service";

        public Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            var emailAddress = "simulate-delivered@notifications.service.gov.uk";
            serviceStatus.CheckParametersUsed = $"Email used - {emailAddress}";
            var vocPersonalisation = new VocSurveyPersonalisation();
            vocPersonalisation.Personalisation.Add("jpprofile", "ServiceCheck");
            vocPersonalisation.Personalisation.Add("clientId", "ServiceCheck ClientId");
            try
            {
                var templateId = ConfigurationManager.AppSettings[Constants.GovUkNotifyTemplateId];
                var apiKey = ConfigurationManager.AppSettings[Constants.GovUkNotifyApiKey];
                var response = clientProxy.SendEmail(apiKey, emailAddress, templateId, this.Convert(vocPersonalisation));

                //Got a response back
                serviceStatus.Status = ServiceState.Amber;
                serviceStatus.Notes = "Success Response";

                if (!string.IsNullOrEmpty(response?.id))
                {
                    serviceStatus.Status = ServiceState.Green;
                    serviceStatus.Notes = string.Empty;
                }
            }
            catch (Exception ex)
            {
                var activityId = Guid.NewGuid().ToString();
                serviceStatus.Notes = $"Exception: Check logs with activity id - {activityId}";
                applicationLogger.ErrorJustLogIt($"Service status check failed for activity id - {activityId}", ex);
            }

            return Task.FromResult(serviceStatus);
        }
        #endregion

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