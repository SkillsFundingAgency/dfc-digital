namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface IConvertTribalEnums
    {
        string[] GetTribalAttendanceModes(string attendanceMode);

        string[] GetTribalStudyModes(string studyMode);
    }
}
