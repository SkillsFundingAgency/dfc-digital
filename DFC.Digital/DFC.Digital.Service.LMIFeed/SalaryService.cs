using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.LMIFeed.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public class SalaryService : ISalaryService
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IAsheHttpClientProxy asheProxy;

        #region ctor

        public SalaryService(IApplicationLogger applicationLogger, IAsheHttpClientProxy asheProxy)
        {
            this.applicationLogger = applicationLogger;
            this.asheProxy = asheProxy;
        }

        #endregion ctor

        #region Implementation of IAsheFeed

        public async Task<JobProfileSalary> GetSalaryBySocAsync(string socCode)
        {
            try
            {
                // Four digit soccode
                var fourDigitSocCode = socCode.Substring(0, 4);
                var response = await asheProxy.EstimatePayMdAsync(fourDigitSocCode);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<JobProfileSalary>();
                }
                else
                {
                    applicationLogger.Warn($"Failed to get the salary information from LMI feed. StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} Content: {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"Failed to get the salary information from LMI feed.", ex);
                return null;
            }
        }

        #endregion Implementation of IAsheFeed
    }
}