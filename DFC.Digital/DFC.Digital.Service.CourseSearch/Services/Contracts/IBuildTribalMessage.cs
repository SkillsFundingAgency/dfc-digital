using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IBuildTribalMessage
    {
        CourseListInput GetCourseSearchInput(string courseName, CourseSearchProperties courseSearchProperties, CourseSearchFilters courseSearchFilters);

        CourseDetailInput GetCourseDetailInput(string courseId);
    }
}
