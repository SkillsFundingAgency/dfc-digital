using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public class SalaryService : ISalaryService, IServiceStatus
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

        #region Implement of IServiceStatus
        private static string ServiceName => "LMI Feed";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new  ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };
            try
            {
                //Plumber
                var checkSOC = "5314";
                serviceStatus.CheckParametersUsed = $"SOC used - {checkSOC}";

                var response = await asheProxy.EstimatePayMdAsync(checkSOC);
                if (response.IsSuccessStatusCode)
                {
                    //Got a response back
                    serviceStatus.Status = ServiceState.Amber;
                    serviceStatus.Notes = "Success Response";

                    var JobProfileSalary = await response.Content.ReadAsAsync<JobProfileSalary>();

                    serviceStatus.Notes = "Response Read";

                    if (JobProfileSalary?.Median != null)
                    {
                        //Manged to read salary information
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.Notes = string.Empty;
                    }
                }
                else
                {
                    serviceStatus.Notes = $"Non Success Response StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"{Constants.ServiceStatusFailedCheckLogsMessage} - {applicationLogger.LogExceptionWithActivityId(Constants.ServiceStatusFailedLogMessage, ex)}";
            }
            return serviceStatus;
        }

        #endregion

        #region Implementation of IAsheFeed

        public async Task<JobProfileSalary> GetSalaryBySocAsync(string socCode)
        {
            if (socCode == null)
            {
                throw new ArgumentNullException(nameof(socCode));
            }

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
                response.EnsureSuccessStatusCode();
            }

            return await Task.FromResult<JobProfileSalary>(default(JobProfileSalary));
        }

        #endregion Implementation of IAsheFeed
    }
}