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
            applicationLogger.Info($" CREATED Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for JobProfile with Title -- {jpData.Title.PadRight(15, ' ')} and Jobprofile Id -- {jpData.JobProfileId.ToString().PadRight(15, ' ')}");
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);

            // Send Messages
            var jsonData = JsonConvert.SerializeObject(jpData);
            try
            {
                applicationLogger.Info($" SENDING Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for JobProfile with Title -- {jpData.Title.PadRight(15, ' ')} and with Jobprofile Id -- {jpData.JobProfileId.ToString().PadRight(15, ' ')} ");

                // Message that send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(jsonData));
                message.ContentType = "application/json";
                message.Label = jpData.Title;
                message.UserProperties.Add("Id", jpData.JobProfileId);
                message.UserProperties.Add("ActionType", actionType);
                message.UserProperties.Add("CType", contentType);
                message.CorrelationId = Guid.NewGuid().ToString();
                await topicClient.SendAsync(message);

                applicationLogger.Info($" SENT SUCCESSFULLY Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for JobProfile with Title -- {jpData.Title.PadRight(15, ' ')}, with Jobprofile Id -- {jpData.JobProfileId.ToString().PadRight(15, ' ')} and with Correlation Id -- {message.CorrelationId.ToString().PadRight(15, ' ')}");
            }
            catch (Exception ex)
            {
                applicationLogger.Info($" FAILED Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for JobProfile with Title -- {jpData.Title.PadRight(15, ' ')} and with Jobprofile Id -- {jpData.JobProfileId.ToString().PadRight(15, ' ')} has an exception \n {ex.Message} ");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }

        public async Task SendOtherRelatedTypeMessages(IEnumerable<RelatedContentItem> relatedContentItems, string contentType, string actionType)
        {
            applicationLogger.Info($" CREATED Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for Item Type -- {contentType.PadRight(15, ' ')} with {relatedContentItems.Count().ToString().PadRight(15, ' ')} message(s)");
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);
            try
            {
                foreach (var relatedContentItem in relatedContentItems)
                {
                    applicationLogger.Info($" SENDING Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message created for Item -- {relatedContentItem.Title.PadRight(15, ' ')} of Type -- {contentType.PadRight(15, ' ')} with Id -- {relatedContentItem.Id.ToString().PadRight(15, ' ')} linked to Job Profile {relatedContentItem.JobProfileTitle.PadRight(15, ' ')} -- {relatedContentItem.JobProfileId.ToString().PadRight(15, ' ')}");

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

                    applicationLogger.Info($" SENT SUCCESSFULLY Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message created for Item -- {relatedContentItem.Title.PadRight(15, ' ')} of Type -- {contentType.PadRight(15, ' ')} with Id -- {relatedContentItem.Id.ToString().PadRight(15, ' ')} linked to Job Profile {relatedContentItem.JobProfileTitle.PadRight(15, ' ')} -- {relatedContentItem.JobProfileId.ToString().PadRight(15, ' ')} with Correlation Id -- {message.CorrelationId.ToString().PadRight(15, ' ')}");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.Info($" FAILED Sitefinity Service Bus {actionType.PadRight(15, ' ').ToUpper()} message for Item Type -- {contentType.PadRight(15, ' ')} with {relatedContentItems.Count().ToString().PadRight(15, ' ')} message(s) has an exception \n {ex.Message}");
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }
    }
}