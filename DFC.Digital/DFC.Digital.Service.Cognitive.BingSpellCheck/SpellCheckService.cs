using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck
{
    public class SpellCheckService : ISpellcheckService, IServiceStatus
    {
        private readonly string bingSpellApiKey = ConfigurationManager.AppSettings[Constants.BingSpellcheckApiKey];
        private readonly string bingSpellEndpoint = ConfigurationManager.AppSettings[Constants.BingSpellcheckRequestEndPoint];

        private readonly IHttpClientService<ISpellcheckService> httpClientService;
        private readonly IApplicationLogger applicationLogger;

        public SpellCheckService(IHttpClientService<ISpellcheckService> httpClientService, IApplicationLogger applicationLogger)
        {
            this.httpClientService = httpClientService;
            this.applicationLogger = applicationLogger;
        }

        #region Implement of IServiceStatus

        private static string ServiceName => "Spell Check Service";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            try
            {
                var checkText = "nursee";
                serviceStatus.CheckParametersUsed = $"Text used - {checkText}";

                var response = await GetSpellCheckResponseAsync(checkText);

                if (response.IsSuccessStatusCode)
                {
                    //Got a response back
                    serviceStatus.Status = ServiceState.Amber;
                    serviceStatus.Notes = "Success Response";

                    var resultsString = await response.Content.ReadAsStringAsync();

                    //Manged to read result information
                    serviceStatus.Notes = "Success Result";

                    dynamic spellSuggestions = JObject.Parse(resultsString);
                    if (spellSuggestions.flaggedTokens.Count > 0)
                    {
                        //got corrections
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.Notes = string.Empty;
                    }
                }
                else
                {
                    serviceStatus.Notes = $"{response.ReasonPhrase}";
                    applicationLogger.Warn($"{nameof(SpellCheckService)}.{nameof(GetCurrentStatusAsync)} : Unsuccessful reason - {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"{Constants.ServiceStatusFailedCheckLogsMessage} - {applicationLogger.LogExceptionWithActivityId(Constants.ServiceStatusFailedLogMessage, ex)}";
            }

            return serviceStatus;
        }

        #endregion Implement of IServiceStatus

        public async Task<SpellcheckResult> CheckSpellingAsync(string term)
        {
            var correctedTerm = term;
            bool hasCorrections = false;

            try
            {
                System.Net.Http.HttpResponseMessage response = await GetSpellCheckResponseAsync(term);
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
            }
            catch (HttpRequestException ex)
            {
                applicationLogger.ErrorJustLogIt($"Failed to GetSpellCheckResponseAsync : {ex.Message}", ex);
            }

            return new SpellcheckResult
            {
                OriginalTerm = term,
                CorrectedTerm = correctedTerm,
                HasCorrected = hasCorrections
            };
        }

        private async Task<System.Net.Http.HttpResponseMessage> GetSpellCheckResponseAsync(string term)
        {
            var requestUri = string.Format(bingSpellEndpoint, term);
            httpClientService.AddHeader(Constants.OcpApimSubscriptionKey, bingSpellApiKey);
            var response = await httpClientService.GetAsync(requestUri, FaultToleranceType.CircuitBreaker);
            return response;
        }
    }
}