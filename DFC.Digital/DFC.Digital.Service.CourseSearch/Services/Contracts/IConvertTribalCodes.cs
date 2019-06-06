using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IConvertTribalCodes
    {
        string[] GetTribalAttendanceModes(CourseType courseType);

        string[] GetTribalStudyModes(CourseHours courseHours);

        string GetEarliestStartDate(StartDate startDate, string earliestStartDate);
    }
}
