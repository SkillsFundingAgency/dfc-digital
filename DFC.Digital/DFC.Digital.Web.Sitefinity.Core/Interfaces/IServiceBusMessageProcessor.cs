using DFC.Digital.Data.Model;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IServiceBusMessageProcessor
    {
        Task SendMessage(JobProfileMessage dynamicContent, string status);

        Task SendMessagesToQueueAsync(JobProfileMessage jpData, int numberOfMessages, TopicClient topicClient, string status);
    }
}
