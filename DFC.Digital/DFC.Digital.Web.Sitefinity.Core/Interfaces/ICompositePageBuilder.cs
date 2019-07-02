using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ICompositePageBuilder
    {
        MicroServicesPublishingPageData GetCompositePageForPageNode(string providerName, Type contentType, Guid itemId);

        IList<string> GetPageContentBlocks(string providerName, Type contentType, Guid itemId);

        string GetMicroServiceEndPointConfigKeyForPageNode(string providerName, Type contentType, Guid itemId);
    }
}
