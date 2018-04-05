using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;

namespace DFC.Digital.Repository.BAUOdataRepository
{
    public class SitefinityOdataRepository : IBauJobProfileOdataRepository
    {
        public async Task<IEnumerable<BauJobProfile>> GetAllJobProfilesBySourcePropertiesAsync()
        {
            bool hasNextPage;
            var nextPage = ConfigurationManager.AppSettings["DFC.Digital.BauJobprofileEndPoint"];
            var bauJobProfilesList = new List<BauJobProfile>();
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


        private async Task<PagedOdataResult<BauJobProfile>> GetNextPage(string nextPageUrl)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(nextPageUrl);

               return JsonConvert.DeserializeObject<PagedOdataResult<BauJobProfile>>(result);
            }
        }
    }
}
