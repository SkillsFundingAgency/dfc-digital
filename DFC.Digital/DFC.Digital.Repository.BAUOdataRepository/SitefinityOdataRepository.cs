using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;

namespace DFC.Digital.Repository.BAUOdataRepository
{
    public class SitefinityOdataRepository : IBauJobProfileOdataRepository
    {
        public async Task<IEnumerable<JobProfileImporting>> GetAllJobProfilesBySourcePropertiesAsync(bool includeRelatedCareers)
        {
            bool hasNextPage;
            var nextPage = ConfigurationManager.AppSettings["DFC.Digital.BauJobprofileEndPoint"]; 

            if (includeRelatedCareers)
            {
                nextPage += "&$expand=RelatedCareers";
            }

            var bauJobProfilesList = new List<JobProfileImporting>();

            do
            {
                var result = await GetNextPage(nextPage);
                bauJobProfilesList.AddRange(result.Value);
                hasNextPage = result.HasNextPage;
                if (hasNextPage)
                {
                    nextPage = result.NextLink;
                }

            } while (hasNextPage);

            return bauJobProfilesList;
        }

        private async Task<PagedOdataResult<JobProfileImporting>> GetNextPage(string nextPageUrl)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(nextPageUrl);

                return JsonConvert.DeserializeObject<PagedOdataResult<JobProfileImporting>>(result);
            }
        }
    }
}
