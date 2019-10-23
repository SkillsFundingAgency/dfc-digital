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

        public ServiceBusMessageProcessor(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public async Task SendJobProfileMessage(JobProfileMessage jpData, string contentType, string actionType)
        {
            var connectionStringServiceBus = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.ConnectionString");
            var topicName = configurationProvider.GetConfig<string>("DFC.Digital.ServiceBus.TopicName");
            var topicClient = new TopicClient(connectionStringServiceBus, topicName);

            // Send Messages
            var jsonData = JsonConvert.SerializeObject(jpData);

            // Message that send to the queue
            var message = new Message(Encoding.UTF8.GetBytes(jsonData));
            Console.WriteLine($"Sending message to queue: {jsonData}");
            message.ContentType = "application/json";
            message.Label = jpData.Title;
            message.UserProperties.Add("Id", jpData.JobProfileId);
            message.UserProperties.Add("EventType", actionType);
            message.UserProperties.Add("CType", contentType);

            await SendMessagesToQueueAsync(message, topicClient);

            Console.ReadKey();

            await topicClient.CloseAsync();
        }

        public async Task SendMessagesToQueueAsync(Message message, TopicClient topicClient)
        {
            try
            {
                // Send the message to the queue
                await topicClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }
    }
}