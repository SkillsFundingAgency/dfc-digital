using System.Collections.Generic;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public interface IFormatContentService
    {
        string GetParagraphText(string openingText, IEnumerable<string> dataItems, string separator);
    }
}