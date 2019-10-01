using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ICompositePageBuilder
    {
        MicroServicesPublishingPageData GetPublishedPage(Type contentType, Guid itemId, string providerName);

        MicroServicesPublishingPageData GetPreviewPage(string name);

        JobProfile GetPublishedDynamicContent(Guid itemId);

        string GetMicroServiceEndPointConfigKeyForPageNode(Type contentType, Guid itemId, string providerName);

        string GetMicroServiceEndPointConfigKeyForDynamicContentNode(Type contentType, Guid itemId, string providerName);
    }
}
