using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseOpportunityBuilder : ICourseOpportunityBuilder
    {
        public IEnumerable<Course> SelectCoursesForJobProfile(IEnumerable<Course> courses)
        {
            if (courses == null)
            {
                return Enumerable.Empty<Course>();
            }

            if (courses.Count() > 2)
            {
                var distinctProviders = courses.Select(c => c.ProviderName).Distinct().Count();
                if (distinctProviders > 1)
                {
                    return courses
                            .OrderBy(c => c.StartDate)
                            .GroupBy(c => c.ProviderName)
                            .Select(g => g.First())
                            .Take(2);
                }
            }

            return courses.Take(2).OrderBy(course => course.StartDate);
        }
    }
}