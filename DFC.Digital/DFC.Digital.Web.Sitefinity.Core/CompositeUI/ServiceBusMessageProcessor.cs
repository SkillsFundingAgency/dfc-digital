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

        public async Task SendJobProfileMessage(JobProfileMessage jpData, string contentType, string actionType)
        {
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);

            // Send Messages
            var jsonData = JsonConvert.SerializeObject(jpData);
            try
            {
                // Message that send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(jsonData));
                message.ContentType = "application/json";
                message.Label = jpData.Title;
                message.UserProperties.Add("Id", jpData.JobProfileId);
                message.UserProperties.Add("ActionType", actionType);
                message.UserProperties.Add("CType", contentType);
                message.CorrelationId = Guid.NewGuid().ToString();
                await topicClient.SendAsync(message);
                applicationLogger.Info($"{Constants.ServiceStatusPassedLogMessage} {jpData.JobProfileId.ToString()}");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }

        public async Task SendOtherRelatedTypeMessages(IEnumerable<RelatedContentItem> relatedContentItems, string contentType, string actionType)
        {
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);
            try
            {
                foreach (var relatedContentItem in relatedContentItems)
                {
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
                    applicationLogger.Info($"{Constants.ServiceStatusPassedLogMessage} {relatedContentItem.JobProfileId}--{relatedContentItem.Id}");
                }
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }
    }
}