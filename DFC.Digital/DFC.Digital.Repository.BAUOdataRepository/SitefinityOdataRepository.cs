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
        private readonly string bauJopProfileEndPoint = ConfigurationManager.AppSettings["DFC.Digital.BauJobprofileEndPoint"];
        public async Task<IEnumerable<BauJobProfile>> GetAllJobProfilesBySourcePropertiesAsync(IEnumerable<string> sourceProperties)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(bauJopProfileEndPoint);
              

                return JsonConvert.DeserializeObject<IEnumerable<BauJobProfile>>(result);
            }
        }
    }
}
