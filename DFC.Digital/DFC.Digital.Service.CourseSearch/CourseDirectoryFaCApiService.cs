using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseDirectoryFaCApiService : ICourseSearchService, IServiceStatus
    {
        private readonly IMapper mapper;
        private readonly FAC.ICourseSearchApiService apiService;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITolerancePolicy tolerancePolicy;

        public CourseDirectoryFaCApiService(IMapper mapper, FAC.ICourseSearchApiService apiService, IApplicationLogger applicationLogger, ITolerancePolicy tolerancePolicy)
        {
            this.mapper = mapper;
            this.apiService = apiService;
            this.applicationLogger = applicationLogger;
            this.tolerancePolicy = tolerancePolicy;
        }

        private static string ServiceName => "Course Search";

        public async Task<CourseDetails> GetCourseDetailsAsync(string courseId, string oppurtunityId)
        {
            var result = await tolerancePolicy.ExecuteAsync(async () => await apiService.GetCourseDetailsAsync(courseId, oppurtunityId).ConfigureAwait(false), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker).ConfigureAwait(false);
            return mapper.Map<CourseDetails>(result);
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(string jobProfileKeywords)
        {
            var result = await tolerancePolicy.ExecuteAsync(async () => await apiService.GetCoursesAsync(jobProfileKeywords).ConfigureAwait(false), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker).ConfigureAwait(false);
            return mapper.Map<IEnumerable<Course>>(result);
        }

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid().ToString() };
            var checkSubject = "maths";

            try
            {
                var apiResult = await apiService.GetCoursesAsync(checkSubject).ConfigureAwait(false);

                //The call worked ok
                serviceStatus.Status = ServiceState.Amber;
                serviceStatus.CheckCorrelationId = "Success Response";

                //We have actual data
                if (apiResult.Any())
                {
                    serviceStatus.Status = ServiceState.Green;
                    serviceStatus.CheckCorrelationId = string.Empty;
                }
                else
                {
                    applicationLogger.Warn($"{nameof(CourseSearchService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Searched for [{checkSubject}]");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(CourseSearchService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Searched for [{checkSubject}]", ex);
            }

            return serviceStatus;
        }

        public async Task<CourseSearchResult> SearchCoursesAsync(CourseSearchProperties courseSearchProperties)
        {
            var result = await tolerancePolicy.ExecuteAsync(async () => await apiService.SearchCoursesAsync(mapper.Map<FindACourseClient.CourseSearchProperties>(courseSearchProperties)).ConfigureAwait(false), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker).ConfigureAwait(false);
            return mapper.Map<CourseSearchResult>(result);
        }
    }
}