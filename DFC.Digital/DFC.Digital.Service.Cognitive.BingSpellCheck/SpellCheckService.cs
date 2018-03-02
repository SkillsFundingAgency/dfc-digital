﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck
{
    public class SpellCheckService : ISpellCheckService
    {
        private readonly string bingSpellApiKey = ConfigurationManager.AppSettings[Constants.BingSpellcheckApiKey];
        private readonly string bingSpellEndpoint = ConfigurationManager.AppSettings[Constants.BingSpellcheckRequestEndPoint];

        private readonly IHttpClientService httpClientService;

        public SpellCheckService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<SpellCheckResult> CheckSpellingAsync(string term)
        {
            var correctedTerm = term;
            bool hasCorrections = false;
            var requestUri = string.Format(bingSpellEndpoint, term);

            var client = GetHttpClient();
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var resultsString = await response.Content.ReadAsStringAsync();
                dynamic spellSuggestions = JObject.Parse(resultsString);
                hasCorrections = spellSuggestions.flaggedTokens.Count > 0;
                if (hasCorrections)
                {
                    foreach (dynamic tokenTerm in spellSuggestions.flaggedTokens)
                    {
                        correctedTerm = correctedTerm.Replace(tokenTerm.token.Value, tokenTerm.suggestions[0].suggestion.Value);
                    }
                }
            }

            return new SpellCheckResult
            {
                OriginalTerm = term,
                CorrectedTerm = correctedTerm,
                HasCorrected = hasCorrections
            };
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = httpClientService.GetHttpClient();
            httpClient.DefaultRequestHeaders.Add(Constants.OcpApimSubscriptionKey, bingSpellApiKey);
            return httpClient;
        }
    }
}