namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IConvertTribalCodes
    {
        string[] GetTribalAttendanceModes(string attendanceMode);

        string[] GetTribalStudyModes(string studyMode);

        string[] GetTribalQualificationLevels(string qualificationLevels);

        string[] GetTribalAttendancePatterns(string attendancePattern);
    }
}
