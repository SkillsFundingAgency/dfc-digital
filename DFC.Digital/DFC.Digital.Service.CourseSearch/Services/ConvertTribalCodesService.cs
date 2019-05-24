using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    /// <summary>
    /// This class get Tribal codes in string arrays
    /// Please check CourseSearchConstants for more info
    /// </summary>
    /// <seealso cref="DFC.Digital.Service.CourseSearchProvider.IConvertTribalCodes" />
    public class ConvertTribalCodesService : IConvertTribalCodes
    {
        public string[] GetTribalAttendanceModes(string attendanceMode)
        {
            if (string.IsNullOrWhiteSpace(attendanceMode))
            {
                return CourseSearchConstants.AllAttendanceModes.Split(',');
            }

            var selectedList = attendanceMode.Split(',');

            if (selectedList.Contains("0", StringComparer.InvariantCultureIgnoreCase))
            {
                return CourseSearchConstants.AllAttendanceModes.Split(',');
            }

            var attendanceList = new List<string>();
            foreach (var attendance in selectedList)
            {
                switch (attendance.ToUpperInvariant())
                {
                    case "1":
                        attendanceList.AddRange(CourseSearchConstants.ClassAttendanceModes.Split(','));
                        break;
                    case "2":
                        attendanceList.AddRange(CourseSearchConstants.WorkAttendanceModes.Split(','));
                        break;
                    case "3":
                        default:
                        attendanceList.AddRange(CourseSearchConstants.DistantAttendanceModes.Split(','));
                        attendanceList.AddRange(CourseSearchConstants.OnlineAttendanceModes.Split(','));
                        break;
                }
            }

            return attendanceList.ToArray();
        }

        public string[] GetTribalStudyModes(string studyMode)
        {
            if (string.IsNullOrWhiteSpace(studyMode) ||
                studyMode.Equals("0"))
            {
                return null;
            }

            var selectedList = studyMode.Split(',');

            if (selectedList.Contains("0"))
            {
                return null;
            }

            var studyModeList = new List<string>();
            foreach (var studyModeItem in selectedList)
            {
                switch (studyModeItem.ToUpperInvariant())
                {
                    case "1":
                        studyModeList.Add(CourseSearchConstants.FulltimeStudyMode);
                        break;
                    case "2":
                        studyModeList.Add(CourseSearchConstants.PartTimeStudyMode);
                        break;
                    case "3":
                    default:
                        studyModeList.Add(CourseSearchConstants.FlexibleStudyMode);
                        break;
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
            foreach (var qualification in selectedList)
            {
                switch (qualification.ToUpperInvariant())
                {
                    case "1":
                        qualificationList.Add(CourseSearchConstants.EntryLevelQualification);
                        break;
                    case "2":
                        qualificationList.Add(CourseSearchConstants.Level1Qualification);
                        break;
                    case "3":
                        qualificationList.Add(CourseSearchConstants.Level2Qualification);
                        break;
                    case "4":
                        qualificationList.Add(CourseSearchConstants.Level3Qualification);
                        break;
                    case "5":
                        qualificationList.Add(CourseSearchConstants.Level4Qualification);
                        break;
                    case "6":
                        qualificationList.Add(CourseSearchConstants.Level5Qualification);
                        break;
                    case "7":
                        qualificationList.Add(CourseSearchConstants.Level6Qualification);
                        break;
                    case "8":
                        qualificationList.Add(CourseSearchConstants.Level7Qualification);
                        break;
                    case "9":
                    default:
                        qualificationList.Add(CourseSearchConstants.Level8Qualification);
                        break;
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
            foreach (var pattern in selectedList)
            {
                switch (pattern.ToUpperInvariant())
                {
                    case "1":
                        patternList.Add(CourseSearchConstants.NormalWorkingHoursPattern);
                        break;
                    case "2":
                        patternList.Add(CourseSearchConstants.DayReleaseBlockPattern);
                        break;
                    case "3":
                    default:
                        patternList.Add(CourseSearchConstants.WeekendPattern);
                        patternList.Add(CourseSearchConstants.EveningPattern);
                        break;
                }
            }

            return patternList.ToArray();
        }
    }
}
