using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ICourseSearchService
    {
        Task<IEnumerable<Course>> GetCoursesAsync(string jobProfileKeywords);

        Task<IEnumerable<Course>> SearchCoursesAsync(CourseSearchRequest courseSearchRequest);
    }
}