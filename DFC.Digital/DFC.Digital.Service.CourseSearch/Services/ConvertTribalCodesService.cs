using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class ConvertTribalCodesService : IConvertTribalCodes
    {
        public string[] GetTribalAttendanceModes(string attendanceMode)
        {
            if (string.IsNullOrWhiteSpace(attendanceMode))
            {
                return CourseSearchConstants.AllAttendanceModes.Split(',');
            }

            var selectedList = attendanceMode.Split(',');

            if (selectedList.Contains("0"))
            {
                return CourseSearchConstants.AllAttendanceModes.Split(',');
            }

            var attendanceList = new List<string>();
            foreach (var attendance in selectedList)
            {
                if (attendance.ToUpperInvariant().Equals("1"))
                {
                    attendanceList.AddRange(CourseSearchConstants.ClassAttendanceModes.Split(','));
                }

                if (attendance.ToUpperInvariant().Equals("2"))
                {
                    attendanceList.AddRange(CourseSearchConstants.WorkAttendanceModes.Split(','));
                }

                if (attendance.ToUpperInvariant().Equals("3"))
                {
                    attendanceList.AddRange(CourseSearchConstants.DistantAttendanceModes.Split(','));
                    attendanceList.AddRange(CourseSearchConstants.OnlineAttendanceModes.Split(','));
                }
            }

            return attendanceList.ToArray();
        }

        public string[] GetTribalStudyModes(string studyMode)
        {
            if (string.IsNullOrWhiteSpace(studyMode) ||
                studyMode.ToUpperInvariant().Equals("0"))
            {
                return null;
            }

            var selectedList = studyMode.Split(',');

            if (selectedList.Contains("0"))
            {
                return null;
            }

            var studyModeList = new List<string>();
            foreach (var attendance in selectedList)
            {
                if (attendance.ToUpperInvariant().Equals("1"))
                {
                    studyModeList.Add(CourseSearchConstants.FulltimeStudyMode);
                }

                if (attendance.ToUpperInvariant().Equals("2"))
                {
                    studyModeList.Add(CourseSearchConstants.PartTimeStudyMode);
                }

                if (attendance.ToUpperInvariant().Equals("3"))
                {
                    studyModeList.Add(CourseSearchConstants.FlexibleStudyMode);
                }
            }

            return studyModeList.ToArray();
        }

        public string[] GetTribalQualificationLevels(string qualificationLevels)
        {
            if (string.IsNullOrWhiteSpace(qualificationLevels) ||
                qualificationLevels.ToUpperInvariant().Equals("0"))
            {
                return CourseSearchConstants.AllQualificationLevels.Split(',');
            }

            var selectedList = qualificationLevels.Split(',');

            if (selectedList.Contains("0"))
            {
                return CourseSearchConstants.AllQualificationLevels.Split(',');
            }

            var qualificationList = new List<string>();
            foreach (var attendance in selectedList)
            {
                if (attendance.ToUpperInvariant().Equals("1"))
                {
                    qualificationList.Add(CourseSearchConstants.EntryLevelQualification);
                }

                if (attendance.ToUpperInvariant().Equals("2"))
                {
                    qualificationList.Add(CourseSearchConstants.Level1Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("3"))
                {
                    qualificationList.Add(CourseSearchConstants.Level2Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("4"))
                {
                    qualificationList.Add(CourseSearchConstants.Level3Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("5"))
                {
                    qualificationList.Add(CourseSearchConstants.Level4Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("6"))
                {
                    qualificationList.Add(CourseSearchConstants.Level5Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("7"))
                {
                    qualificationList.Add(CourseSearchConstants.Level6Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("8"))
                {
                    qualificationList.Add(CourseSearchConstants.Level7Qualification);
                }

                if (attendance.ToUpperInvariant().Equals("9"))
                {
                    qualificationList.Add(CourseSearchConstants.Level8Qualification);
                }
            }

            return qualificationList.ToArray();
        }

        public string[] GetTribalAttendancePatterns(string attendancePattern)
        {
            if (string.IsNullOrWhiteSpace(attendancePattern))
            {
                return null;
            }

            var selectedList = attendancePattern.Split(',');

            if (selectedList.Contains("0"))
            {
                return null;
            }

            var patternList = new List<string>();
            foreach (var attendance in selectedList)
            {
                if (attendance.ToUpperInvariant().Equals("1"))
                {
                    patternList.Add(CourseSearchConstants.NormalWorkingHoursPattern);
                }

                if (attendance.ToUpperInvariant().Equals("2"))
                {
                    patternList.Add(CourseSearchConstants.DayReleaseBlockPattern);
                }

                if (attendance.ToUpperInvariant().Equals("3"))
                {
                    patternList.Add(CourseSearchConstants.WeekendPattern);
                    patternList.Add(CourseSearchConstants.EveningPattern);
                }
            }

            return patternList.ToArray();
        }
    }
}
