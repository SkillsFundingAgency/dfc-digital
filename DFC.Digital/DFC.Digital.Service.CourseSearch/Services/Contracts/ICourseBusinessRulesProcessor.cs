using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ICourseBusinessRulesProcessor
    {
        string GetEarliestStartDate(StartDate startDate, DateTime earliestStartDate);
    }
}
