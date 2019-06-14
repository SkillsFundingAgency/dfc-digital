using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ITribalMessageBuilder
    {
        CourseListInput GetCourseSearchInput(CourseSearchProperties courseSearchProperties);

        CourseDetailInput GetCourseDetailInput(string courseId);
    }
}
