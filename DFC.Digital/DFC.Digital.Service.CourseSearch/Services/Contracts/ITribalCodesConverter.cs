using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ITribalCodesConverter
    {
        string[] GetTribalAttendanceModes(CourseType courseType);

        string[] GetTribalStudyModes(CourseHours courseHours);
    }
}
