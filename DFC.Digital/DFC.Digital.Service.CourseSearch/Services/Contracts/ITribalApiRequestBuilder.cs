using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ITribalApiRequestBuilder
    {
        CourseListInput BuildCourseSearchRequest(string courseName, CourseSearchProperties courseSearchProperties = null);

        CourseDetailInput BuildCourseDetailRequest(string courseId);
    }
}