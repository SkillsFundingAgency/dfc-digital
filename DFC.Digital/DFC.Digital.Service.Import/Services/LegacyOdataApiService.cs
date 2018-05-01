using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Import
{
    public class LegacyOdataApiService : IOdataApiService<LegacyJobProfile>
    {
        private const string CONTENTTYPE = "application/json";

        private readonly ITokenClient tokenClient;
        private readonly IApplicationLogger logger;
        private readonly IHttpClientService<LegacyJobProfile> httpClient;
        private bool hasInitialised = false;

        public LegacyOdataApiService(ITokenClient tokenClient, IHttpClientService<LegacyJobProfile> httpClient, IApplicationLogger logger)
        {
            this.tokenClient = tokenClient;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<PagedOdataResult<LegacyJobProfile>> GetResultAsync(Uri requestUri)
        {
            if (!hasInitialised)
            {
                await SetupHttpClient();
            }

            var result = await httpClient.GetAsync(requestUri.AbsoluteUri);
            return JsonConvert.DeserializeObject<PagedOdataResult<LegacyJobProfile>>(await result.Content.ReadAsStringAsync());
        }

        public Task<string> PostAsync(Uri requestUri, LegacyJobProfile data)
        {
            throw new NotImplementedException();
        }

        public Task<string> PutAsync(Uri requestUri, string relatedEntityLink)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string requestUri)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<LegacyJobProfile>> GetAllAsync(Uri uri)
        {
            bool hasNextPage;
            var bauJobProfiles = new List<LegacyJobProfile>();
            var nextPage = uri;
            do
            {
                var result = await GetResultAsync(uri);
                bauJobProfiles.AddRange(result.Value);
                hasNextPage = result.HasNextPage;
                if (hasNextPage)
                {
                    nextPage = new Uri(result.NextLink);
                }
            }
            while (hasNextPage);

            return bauJobProfiles;
        }

        private async Task SetupHttpClient()
        {
            httpClient.SetBearerToken(await tokenClient.GetAccessTokenAsync());
            httpClient.AddHeader("X-SF-Service-Request", "true");
            httpClient.Accept(new MediaTypeWithQualityHeaderValue(CONTENTTYPE));
        }
    }
}