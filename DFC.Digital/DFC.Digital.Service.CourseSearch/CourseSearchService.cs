using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.Converters;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseSearchService : ICourseSearchService, IServiceStatus
    {
        private readonly ICourseOpportunityBuilder courseOpportunityBuilder;
        private readonly IAuditRepository auditRepository;
        private readonly IServiceHelper serviceHelper;
        private readonly IApplicationLogger applicationLogger;

        public CourseSearchService(ICourseOpportunityBuilder courseOpportunityBuilder, IServiceHelper serviceHelper, IAuditRepository auditRepository, IApplicationLogger applicationLogger)
        {
            this.courseOpportunityBuilder = courseOpportunityBuilder;
            this.auditRepository = auditRepository;
            this.serviceHelper = serviceHelper;
            this.applicationLogger = applicationLogger;
        }

        private string ServiceName => "Course Search";

        public ServiceStatus GetCurrentStatus()
        {
            return new ServiceStatus { Name = ServiceName, Status = ServiceState.Green, Notes = string.Empty };
        }

        public IEnumerable<Course> GetCourses(string jobprofileKeywords)
        {
            var request = MessageConverter.GetCourseListInput(jobprofileKeywords);
            auditRepository.CreateAudit(request);

            //if the the call to the courses API fails for anyreason we should log and continue as if there are no courses available.
            try
            {
                var apiResult = serviceHelper.Use<ServiceInterface, CourseListOutput>(x => x.CourseList(request), Constants.CourseSerachEndpointConfigName);
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
    }
}