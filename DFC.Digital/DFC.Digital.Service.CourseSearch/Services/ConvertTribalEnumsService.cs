using DFC.Digital.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class ConvertTribalEnumsService : IConvertTribalEnums
    {
        public string[] GetTribalAttendanceModes(string attendanceMode)
        {
            if (string.IsNullOrWhiteSpace(attendanceMode))
            {
                return Convert.ToString(ConfigurationManager.AppSettings[Constants.CourseSearchAttendanceModes])?.Split(',');
            }

            var selectedList = attendanceMode.Split(',');

            if (selectedList.Contains("0"))
            {
                return Convert.ToString(ConfigurationManager.AppSettings[Constants.CourseSearchAttendanceModes])?.Split(',');
            }

            var attendanceList = new List<string>();
            foreach (var attendance in selectedList)
            {
                if (attendance.ToLowerInvariant().Equals("1"))
                {
                    attendanceList.AddRange(CourseSearchConstants.ClassAttendanceModes.Split(','));
                }

                if (attendance.ToLowerInvariant().Equals("2"))
                {
                    attendanceList.AddRange(CourseSearchConstants.WorkAttendanceModes.Split(','));
                }

                if (attendance.ToLowerInvariant().Equals("3"))
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
                studyMode.ToLowerInvariant().Equals("0"))
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
                if (attendance.ToLowerInvariant().Equals("1"))
                {
                    studyModeList.Add(CourseSearchConstants.FullTimeStudyMode);
                }

                if (attendance.ToLowerInvariant().Equals("2"))
                {
                    studyModeList.Add(CourseSearchConstants.PartTimeStudyMode);
                }

                if (attendance.ToLowerInvariant().Equals("3"))
                {
                    studyModeList.Add(CourseSearchConstants.FlexibleStudyMode);
                }
            }

            return studyModeList.ToArray();
        }
    }
}
