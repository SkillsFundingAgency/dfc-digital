using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    /// <summary>
    /// This class get Tribal codes in string arrays
    /// Please check CourseSearchConstants for more info
    /// </summary>
    /// <seealso cref="DFC.Digital.Service.CourseSearchProvider.IConvertTribalCodes" />
    public class ConvertTribalCodes : IConvertTribalCodes
    {
        public string[] GetTribalAttendanceModes(CourseType courseType)
        {
            switch (courseType)
            {
                case CourseType.ClassroomBased:
                    return CourseSearchConstants.AllAttendanceModes;
                case CourseType.DistanceLearning:
                    return CourseSearchConstants.DistantAttendanceModes;
                case CourseType.Online:
                    return CourseSearchConstants.OnlineAttendanceModes;
                case CourseType.WorkBased:
                    return CourseSearchConstants.WorkAttendanceModes;
                case CourseType.All:
                default:
                    return CourseSearchConstants.AllAttendanceModes;
            }
        }

        public string[] GetTribalStudyModes(CourseHours courseHours)
        {
            switch (courseHours)
            {
                case CourseHours.FullTime:
                    return new[] { CourseSearchConstants.FulltimeStudyMode };
                case CourseHours.PartTime:
                    return new[] { CourseSearchConstants.PartTimeStudyMode };
                case CourseHours.Flexible:
                    return new[] { CourseSearchConstants.FlexibleStudyMode };
                case CourseHours.All:
                default:
                    return null;
            }
        }

        public string GetEarliestStartDate(StartDate startDate, string earliestStartDate)
        {
            switch (startDate)
            {
                case StartDate.Anytime:
                    return null;
                case StartDate.FromToday:
                    return DateTime.Now.ToString("yyyy-MM-dd");
                case StartDate.SelectDateFrom:
                default:
                    if (DateTime.TryParse(earliestStartDate, out var choseDate))
                    {
                        return choseDate.ToString("yyyy-MM-dd");
                    }

                    return null;
            }
        }
    }
}
