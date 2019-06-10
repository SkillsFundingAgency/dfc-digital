using System;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ICourseBusinessRules
    {
        string GetEarliestStartDate(StartDate startDate, DateTime earliestStartDate);
    }
}
