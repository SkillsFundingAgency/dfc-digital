using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    /// <summary>
    /// This class get Tribal codes in string arrays
    /// Please check CourseSearchConstants for more info
    /// </summary>
    /// <seealso cref="ITribalCodesConverter" />
    public class TribalCodesConverter : ITribalCodesConverter
    {
        public string[] GetTribalAttendanceModes(CourseType courseType)
        {
            switch (courseType)
            {
                case CourseType.ClassroomBased:
                    return CourseSearchConstants.ClassAttendanceModes();
                case CourseType.DistanceLearning:
                    return CourseSearchConstants.DistantAttendanceModes();
                case CourseType.Online:
                    return CourseSearchConstants.OnlineAttendanceModes();
                case CourseType.WorkBased:
                    return CourseSearchConstants.WorkAttendanceModes();
                case CourseType.All:
                default:
                    return CourseSearchConstants.AllAttendanceModes();
            }
        }

        public string[] GetTribalStudyModes(CourseHours courseHours)
        {
            switch (courseHours)
            {
                case CourseHours.Fulltime:
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
    }
}
