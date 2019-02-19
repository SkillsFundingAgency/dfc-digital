using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IBuildTribalMessage
    {
        CourseListInput GetCourseSearchInput(CourseSearchRequest request);
    }
}
