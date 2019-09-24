using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ICompositeJobProfileContentBuilder
    {
        MicroServicesPublishingJobProfileContentBuilder GetPublishedContent(Type contentType, Guid itemId, string providerName);

        MicroServicesPublishingJobProfileContentBuilder GetPreviewContent(string providerName);

        string GetMicroServiceEndPointConfigKeyForContentNode(Type contentType, Guid itemId, string providerName);
    }
}
