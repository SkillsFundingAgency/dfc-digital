using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ICourseBusinessRules
    {
        DateTime GetEarliestStartDate(DateTime inputDate);
    }
}
