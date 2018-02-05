using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.LMIFeed.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public class SalaryService : ISalaryService
    {
        readonly IApplicationLogger _applicationLogger;
        readonly IAsheHttpClientProxy _asheProxy;

        #region ctor

        public SalaryService(IApplicationLogger applicationLogger, IAsheHttpClientProxy asheProxy)
        {
            this._applicationLogger = applicationLogger;
            this._asheProxy = asheProxy;
        }

        #endregion ctor

        #region Implementation of IAsheFeed

        public async Task<JobProfileSalary> GetSalaryBySocAsync(string socCode)
        {
            try
            {
                // Four digit soccode
                var fourDigitSocCode = socCode.Substring(0, 4);
                var response = await _asheProxy.EstimatePayMdAsync(fourDigitSocCode);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<JobProfileSalary>();
                }
                else
                {
                    _applicationLogger.Warn($"Failed to get the salary information from LMI feed. StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase} Content: {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _applicationLogger.ErrorJustLogIt($"Failed to get the salary information from LMI feed.", ex);
                return null;
            }
        }

        #endregion Implementation of IAsheFeed
    }
}