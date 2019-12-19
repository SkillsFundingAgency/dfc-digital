using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.FindACourseClient.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseDirectoryFaCApiService : ICourseSearchService, IServiceStatus
    {
        private readonly IMapper mapper;
        private readonly ICourseSearchApiService apiService;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITolerancePolicy tolerancePolicy;

        public CourseDirectoryFaCApiService(IMapper mapper, ICourseSearchApiService apiService, IApplicationLogger applicationLogger, ITolerancePolicy tolerancePolicy)
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
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            var checkSubject = "maths";
            serviceStatus.CheckParametersUsed = $"Searched for - {checkSubject}";

            try
            {
                var apiResult = await apiService.GetCoursesAsync(checkSubject).ConfigureAwait(false);

                //The call worked ok
                serviceStatus.Status = ServiceState.Amber;
                serviceStatus.Notes = "Success Response";

                //We have actual data
                if (apiResult.Any())
                {
                    serviceStatus.Status = ServiceState.Green;
                    serviceStatus.Notes = string.Empty;
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"{Constants.ServiceStatusFailedCheckLogsMessage} - {applicationLogger.LogExceptionWithActivityId(Constants.ServiceStatusFailedLogMessage, ex)}";
            }

            return serviceStatus;
        }

        public async Task<CourseSearchResult> SearchCoursesAsync(CourseSearchProperties courseSearchProperties)
        {
            var result = await tolerancePolicy.ExecuteAsync(async () => await apiService.SearchCoursesAsync(mapper.Map<FindACourseClient.Models.ExternalInterfaceModels.CourseSearchProperties>(courseSearchProperties)).ConfigureAwait(false), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker).ConfigureAwait(false);
            return mapper.Map<CourseSearchResult>(result);
        }
    }
}