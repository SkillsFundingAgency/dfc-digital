using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseBusinessRulesProcessor : ICourseBusinessRulesProcessor
    {
        public string GetEarliestStartDate(StartDate startDate, DateTime earliestStartDate)
        {
            switch (startDate)
            {
                case StartDate.FromToday:
                    return DateTime.Now.ToString("yyyy-MM-dd");
                case StartDate.SelectDateFrom:
                    return CalculateEarliestStartDate(earliestStartDate).ToString("yyyy-MM-dd");
                case StartDate.Anytime:
                default:
                    return null;
            }
        }

        private DateTime CalculateEarliestStartDate(DateTime inputDate)
        {
            var earliestDate = DateTime.Now.AddYears(-1);
            var latestDate = DateTime.Now.AddYears(1);
            return inputDate >= earliestDate && inputDate < latestDate ? inputDate :
                inputDate < earliestDate ? earliestDate :
                latestDate;
        }
    }
}