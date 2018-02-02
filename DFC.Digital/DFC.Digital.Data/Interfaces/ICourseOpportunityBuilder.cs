using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface ICourseOpportunityBuilder
    {
        IEnumerable<Course> SelectCoursesForJobProfile(IEnumerable<Course> courses);
    }
}