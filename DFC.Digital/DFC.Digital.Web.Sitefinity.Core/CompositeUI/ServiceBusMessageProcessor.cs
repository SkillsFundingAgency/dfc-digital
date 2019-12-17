using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class ServiceBusMessageProcessor : IServiceBusMessageProcessor
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly IApplicationLogger applicationLogger;

        public ServiceBusMessageProcessor(IApplicationLogger applicationLogger, IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            this.applicationLogger = applicationLogger;
        }

        public async Task SendContentPageMessage(MicroServicesPublishingPageData contentPageData, string contentType, string actionType)
        {
            applicationLogger.Info($" CREATED service bus message for sitefinity event {actionType.ToUpper()} on ContentPage with Title -- {contentPageData.CanonicalName} and Id -- {contentPageData.Id.ToString()}");
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);

            // Send Messages
            var jsonData = JsonConvert.SerializeObject(contentPageData);
            try
            {
                applicationLogger.Info($" SENDING service bus message for sitefinity event {actionType.ToUpper()} on ContentPage with Title -- {contentPageData.CanonicalName} and Id -- {contentPageData.Id.ToString()} ");

                // Message that send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(jsonData));
                message.ContentType = "application/json";
                message.Label = contentPageData.CanonicalName;
                message.UserProperties.Add("Id", contentPageData.Id);
                message.UserProperties.Add("ActionType", actionType);
                message.UserProperties.Add("CType", contentType);
                message.CorrelationId = Guid.NewGuid().ToString();
                await topicClient.SendAsync(message);

                applicationLogger.Info($" SENT SUCCESSFULLY service bus message for sitefinity event {actionType.ToUpper()} on ContentPage with Title -- {contentPageData.CanonicalName} with Id -- {contentPageData.Id.ToString()} and with Correlation Id -- {message.CorrelationId.ToString()}");
            }
            catch (Exception ex)
            {
                applicationLogger.Info($" FAILED service bus message for sitefinity event {actionType.ToUpper()} on ContentPage with Title -- {contentPageData.CanonicalName} with Id -- {contentPageData.Id.ToString()} has an exception \n {ex.Message} ");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }

        public async Task SendJobProfileMessage(JobProfileMessage jpData, string contentType, string actionType)
        {
            applicationLogger.Info($" CREATED service bus message for sitefinity event {actionType.ToUpper()} on JobProfile with Title -- {jpData.Title} and Jobprofile Id -- {jpData.JobProfileId.ToString()}");
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);

            // Send Messages
            var jsonData = JsonConvert.SerializeObject(jpData);
            try
            {
                applicationLogger.Info($" SENDING service bus message for sitefinity event {actionType.ToUpper()} on JobProfile with Title -- {jpData.Title} and with Jobprofile Id -- {jpData.JobProfileId.ToString()} ");

                // Message that send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(jsonData));
                message.ContentType = "application/json";
                message.Label = jpData.Title;
                message.UserProperties.Add("Id", jpData.JobProfileId);
                message.UserProperties.Add("ActionType", actionType);
                message.UserProperties.Add("CType", contentType);
                message.CorrelationId = Guid.NewGuid().ToString();
                await topicClient.SendAsync(message);

                applicationLogger.Info($" SENT service bus message for sitefinity event {actionType.ToUpper()} on JobProfile with Title -- {jpData.Title} with Jobprofile Id -- {jpData.JobProfileId.ToString()} and with Correlation Id -- {message.CorrelationId.ToString()}");
            }
            catch (Exception ex)
            {
                applicationLogger.Info($" FAILED service bus message for sitefinity event {actionType.ToUpper()} on JobProfile with Title -- {jpData.Title} and with Jobprofile Id -- {jpData.JobProfileId.ToString()} has an exception \n {ex.Message} ");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }

        public async Task SendOtherRelatedTypeMessages(IEnumerable<RelatedContentItem> relatedContentItems, string contentType, string actionType)
        {
            applicationLogger.Info($" CREATED service bus message for sitefinity event {actionType.ToUpper()} on Item of Type -- {contentType.ToUpper()} with {relatedContentItems.Count().ToString()} message(s)");
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);
            try
            {
                foreach (var relatedContentItem in relatedContentItems)
                {
                    applicationLogger.Info($" SENDING service bus message for sitefinity event {actionType.ToUpper()}  on Item -- {relatedContentItem.Title} of Type -- {contentType} with Id -- {relatedContentItem.Id.ToString()} linked to Job Profile {relatedContentItem.JobProfileTitle} -- {relatedContentItem.JobProfileId.ToString()}");

                    // Send Messages
                    var jsonData = JsonConvert.SerializeObject(relatedContentItem);

                    // Message that send to the queue
                    var message = new Message(Encoding.UTF8.GetBytes(jsonData));
                    message.ContentType = "application/json";
                    message.Label = relatedContentItem.Title;
                    message.UserProperties.Add("Id", $"{relatedContentItem.JobProfileId}--{relatedContentItem.Id}");
                    message.UserProperties.Add("ActionType", actionType);
                    message.UserProperties.Add("CType", contentType);
                    message.CorrelationId = Guid.NewGuid().ToString();
                    await topicClient.SendAsync(message);

                    applicationLogger.Info($" SENT service bus message for sitefinity event {actionType.ToUpper()} on Item -- {relatedContentItem.Title} of Type -- {contentType} with Id -- {relatedContentItem.Id.ToString()} linked to Job Profile {relatedContentItem.JobProfileTitle} -- {relatedContentItem.JobProfileId.ToString()} with Correlation Id -- {message.CorrelationId.ToString()}");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.Info($" FAILED service bus message for sitefinity event {actionType.ToUpper()} on Item of Type -- {contentType} with {relatedContentItems.Count().ToString()} message(s) has an exception \n {ex.Message}");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }
    }
}