using System.Collections.Generic;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IConvertTribalCodes
    {
        string[] GetTribalAttendanceModes(IEnumerable<string> attendanceMode);

        string[] GetTribalStudyModes(string studyMode);

        string[] GetTribalQualificationLevels(string qualificationLevels);

        string[] GetTribalAttendancePatterns(string attendancePattern);
    }
}
