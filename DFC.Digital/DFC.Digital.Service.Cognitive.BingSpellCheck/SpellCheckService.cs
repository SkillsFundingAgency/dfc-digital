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
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid() };
            var checkText = "nursee";

            try
            {
                var response = await GetSpellCheckResponseAsync(checkText);

                if (response.IsSuccessStatusCode)
                {
                    //Got a response back
                    serviceStatus.Status = ServiceState.Amber;
                    var resultsString = await response.Content.ReadAsStringAsync();

                    dynamic spellSuggestions = JObject.Parse(resultsString);
                    if (spellSuggestions.flaggedTokens.Count > 0)
                    {
                        //got corrections
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.CheckCorrelationId = Guid.Empty;
                    }
                }
                else
                {
                    applicationLogger.Warn($"{nameof(SpellCheckService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Text used [{checkText}] - Unsuccessful reason [{response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}]");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(SpellCheckService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Text used [{checkText}]", ex);
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

                return new SpellcheckResult
                {
                    OriginalTerm = term,
                    CorrectedTerm = correctedTerm,
                    HasCorrected = hasCorrections
                };
            }
            catch (LoggedException)
            {
                //Ignore already logged exception and return default.
                return new SpellcheckResult
                {
                    OriginalTerm = term,
                    HasCorrected = false
                };
            }
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