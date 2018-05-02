using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.Import
{
    public class ImportLegacyJobProfileCsvParser : IImportInputParser<LegacyJobProfile>
    {
        public IEnumerable<LegacyJobProfile> Parse(IEnumerable<string> content)
        {
            return content
                .Skip(1) //Skip header
                .Select(row => new LegacyJobProfile
                {
                    UrlName = row.Split(',')[0],
                    CourseKeywords = row.Split(',')[1],
                });
        }
    }
}