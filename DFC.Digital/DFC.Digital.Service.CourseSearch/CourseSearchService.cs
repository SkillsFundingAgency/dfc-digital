using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseSearchService : ICourseSearchService, IServiceStatus
    {
        private readonly ICourseOpportunityBuilder courseOpportunityBuilder;
        private readonly IAuditRepository auditRepository;
        private readonly IServiceHelper serviceHelper;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITolerancePolicy tolerancePolicy;
        private readonly IBuildTribalMessage buildTribalMessage;

        public CourseSearchService(
            ICourseOpportunityBuilder courseOpportunityBuilder,
            IServiceHelper serviceHelper,
            IAuditRepository auditRepository,
            IApplicationLogger applicationLogger,
            ITolerancePolicy tolerancePolicy,
            IBuildTribalMessage buildTribalMessage)
        {
            this.courseOpportunityBuilder = courseOpportunityBuilder;
            this.auditRepository = auditRepository;
            this.serviceHelper = serviceHelper;
            this.applicationLogger = applicationLogger;
            this.tolerancePolicy = tolerancePolicy;
            this.buildTribalMessage = buildTribalMessage;
        }

        private static string ServiceName => "Course Search";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };

            var checkSubject = "maths";
            serviceStatus.CheckParametersUsed = $"Searched for - {checkSubject}";

            try
            {
                var request = MessageConverter.GetCourseListInput(checkSubject);
                var apiResult = await serviceHelper.UseAsync<ServiceInterface, CourseListOutput>(async x => await tolerancePolicy.ExecuteAsync(() => x.CourseListAsync(request), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker), Constants.CourseSearchEndpointConfigName);

                //The call worked ok
                serviceStatus.Status = ServiceState.Amber;
                serviceStatus.Notes = "Success Response";

                //We have actual data
                if (apiResult.CourseListResponse.CourseDetails.Any())
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

        public async Task<IEnumerable<Course>> GetCoursesAsync(string jobProfileKeywords)
        {
            if (jobProfileKeywords == null)
            {
                return null;
            }

            var request = MessageConverter.GetCourseListInput(jobProfileKeywords);
            auditRepository.CreateAudit(request);

            //if the the call to the courses API fails for anyreason we should log and continue as if there are no courses available.
            try
            {
                var apiResult = await serviceHelper.UseAsync<ServiceInterface, CourseListOutput>(async x => await tolerancePolicy.ExecuteAsync(() => x.CourseListAsync(request), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker), Constants.CourseSearchEndpointConfigName);
                auditRepository.CreateAudit(apiResult);
                var result = apiResult?.ConvertToCourse();
                var filteredResult = courseOpportunityBuilder.SelectCoursesForJobProfile(result);
                return filteredResult;
            }
            catch (Exception ex)
            {
                auditRepository.CreateAudit(ex);
                applicationLogger.ErrorJustLogIt("Getting courses Failed - ", ex);
                return Enumerable.Empty<Course>();
            }
        }

        public async Task<CourseSearchResult> SearchCoursesAsync(string courseName, CourseSearchProperties courseSearchProperties)
        {
            if (string.IsNullOrWhiteSpace(courseName))
            {
                return null;
            }

            var response = new CourseSearchResult();
            var request = buildTribalMessage.GetCourseSearchInput(courseName, courseSearchProperties);
            auditRepository.CreateAudit(request);

            //if the the call to the courses API fails for anyreason we should log and continue as if there are no courses available.
            try
            {
                var apiResult = await serviceHelper.UseAsync<ServiceInterface, CourseListOutput>(async x => await tolerancePolicy.ExecuteAsync(() => x.CourseListAsync(request), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker), Constants.CourseSearchEndpointConfigName);
                auditRepository.CreateAudit(apiResult);
                response.ResultProperties.TotalPages = Convert.ToInt32(apiResult?.CourseListResponse?.ResultInfo?.NoOfPages);
                response.ResultProperties.TotalResultCount = Convert.ToInt32(apiResult?.CourseListResponse?.ResultInfo?.NoOfRecords);
                response.ResultProperties.Page = Convert.ToInt32(apiResult?.CourseListResponse?.ResultInfo?.PageNo);
                response.Courses = apiResult?.ConvertToSearchCourse();
                response.ResultProperties.OrderedBy = courseSearchProperties.OrderedBy;
            }
            catch (Exception ex)
            {
                auditRepository.CreateAudit(ex);
                applicationLogger.ErrorJustLogIt("search courses Failed - ", ex);
                response.Courses = Enumerable.Empty<Course>();
            }

            return response;
        }

        public async Task<CourseDetails> GetCourseDetailsAsync(string courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId))
            {
                return null;
            }

            var response = new CourseDetails();
            var request = buildTribalMessage.GetCourseDetailInput(courseId);
            auditRepository.CreateAudit(request);

            //if the the call to the courses API fails for anyreason we should log and continue as if there are no courses available.
            try
            {
                var apiResult = await serviceHelper.UseAsync<ServiceInterface, CourseDetailOutput>(async x => await tolerancePolicy.ExecuteAsync(() => x.CourseDetailAsync(request), Constants.CourseSearchEndpointConfigName, FaultToleranceType.CircuitBreaker), Constants.CourseSearchEndpointConfigName);
                auditRepository.CreateAudit(apiResult);
                response = apiResult?.ConvertToCourseDetails();
            }
            catch (Exception ex)
            {
                auditRepository.CreateAudit(ex);
                applicationLogger.ErrorJustLogIt("search courses Failed - ", ex);
            }

            return response;
        }
    }
}