using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface IQueryStringBuilder<in T>
    {
        string BuildPathAndQueryString(string path, T queryParameters);
    }
}
