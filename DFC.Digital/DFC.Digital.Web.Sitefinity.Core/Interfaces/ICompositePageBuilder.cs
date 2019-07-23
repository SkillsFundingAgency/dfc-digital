using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ICompositePageBuilder
    {
        MicroServicesPublishingPageData GetCompositePublishedPage(Type contentType, Guid itemId, string providerName);

        MicroServicesPublishingPageData GetCompositePreviewPage(string name);

        string GetPageContentBlocks(Type contentType, Guid itemId, string providerName);

        string GetMicroServiceEndPointConfigKeyForPageNode(Type contentType, Guid itemId, string providerName);
    }
}
