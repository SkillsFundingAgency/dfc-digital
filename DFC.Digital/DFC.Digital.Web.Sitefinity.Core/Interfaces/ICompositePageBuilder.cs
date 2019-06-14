using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ICompositePageBuilder
    {
        CompositePageData GetCompositePageForPageNode(string providerName, Type contentType, Guid itemId);

        IList<string> GetPageContentBlocks(string providerName, Type contentType, Guid itemId);
    }
}
