using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public class FormatContentService : IFormatContentService
    {
        public string GetParagraphText(string openingText, IEnumerable<string> dataItems, string separator)
        {
            if (dataItems.Any())
            {
                switch (dataItems.Count())
                {
                    case 1:
                        return $"{openingText} {dataItems.FirstOrDefault()}.";
                    case 2:
                        return $"{openingText} {string.Join($" {separator} ", dataItems)}.";
                        default:
                            return
                                $"{openingText} {string.Join(", ", dataItems.Take(dataItems.Count() - 1))} {separator} {dataItems.Last()}.";
                }
            }

            return string.Empty;
        }
    }
}