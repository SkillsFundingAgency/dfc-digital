using DFC.Digital.Core;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.Converters;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseSearchService : ICourseSearchService
    {
        private readonly ICourseOpportunityBuilder courseOpportunityBuilder;
        private readonly IAuditRepository auditRepository;
        private readonly IServiceHelper serviceHelper;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITolerancePolicy tolerancePolicy;

        public CourseSearchService(
            ICourseOpportunityBuilder courseOpportunityBuilder, 
            IServiceHelper serviceHelper, 
            IAuditRepository auditRepository, 
            IApplicationLogger applicationLogger,
            ITolerancePolicy tolerancePolicy)
        {
            this.courseOpportunityBuilder = courseOpportunityBuilder;
            this.auditRepository = auditRepository;
            this.serviceHelper = serviceHelper;
            this.applicationLogger = applicationLogger;
            this.tolerancePolicy = tolerancePolicy;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(string jobprofileKeywords)
        {
            var request = MessageConverter.GetCourseListInput(jobprofileKeywords);
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
    }
}