using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public interface IFormatContentService
    {
        string GetParagraphText(string openingText, IEnumerable<string> dataItems, string separator);
    }
}