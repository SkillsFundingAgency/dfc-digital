using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IConvertTribalCodes
    {
        string[] GetTribalAttendanceModes(CourseType courseType);

        string[] GetTribalStudyModes(CourseHours courseHours);
    }
}
