namespace DFC.Digital.Service.CourseSearchProvider
{
    public static class CourseSearchConstants
    {
        //https://coursedirectoryproviderportal.org.uk/Content/Files/Help/Course%20Directory%20API.pdf
        //6.5. Attendance Modes
        //Code Description
        //AM1 Location / campus
        //AM2 Face-to-face(non-campus)
        //AM3 Work-based
        //AM4 Mixed Mode
        //AM5 Distance with attendance
        //AM6 Distance without attendance
        //AM7 Online without attendance
        //AM8 Online with attendance
        //AM9 Not known
        public const string AllAttendanceModes = "AM1,AM2,AM3,AM4,AM5,AM6,AM7,AM8,AM9";
        public const string ClassAttendanceModes = "AM1,AM2,AM4,AM9";
        public const string OnlineAttendanceModes = "AM4,AM7,AM8,AM9";
        public const string DistantAttendanceModes = "AM4,AM5,AM6,AM9";
        public const string WorkAttendanceModes = "AM3,AM4,AM6,AM7,AM9";

        //6.4. Study Modes
        //Code Description
        //SM1 Full time
        //SM2 Part time
        //SM3 Part of a full-time program
        //SM4 Flexible
        //SM5 Not known
        public const string FulltimeStudyMode = "SM1";
        public const string PartTimeStudyMode = "SM2";
        public const string FlexibleStudyMode = "SM4";

        //6.6. Attendance Patterns
        //Code Description
        //AP1 Daytime/working hours
        //AP2 Day/Block release
        //AP3 Evening
        //AP4 Twilight
        //AP5 Weekend
        //AP6 Customised
        //AP7 Not known
        //AP8 Not applicable
        public const string NormalWorkingHoursPattern = "AP1";
        public const string DayReleaseBlockPattern = "AP2";
        public const string EveningPattern = "AP3";
        public const string WeekendPattern = "AP5";

        //Qualification Levels
        //Code Description
        //LV0 Entry Level
        //LV1 Level 1
        //LV2 Level 2
        //LV3 Level 3
        //LV4 Level 4
        //LV5 Level 5
        //LV6 Level 6
        //LV7 Level 7
        //LV8 Level 8
        //LV9 Higher Level
        //LVNA Unknown/not applicable
        public const string AllQualificationLevels = "LV0,LV1,LV2,LV3,LV4,LV5,LV6,LV7,LV8,LV9,LVNA";
        public const string EntryLevelQualification = "LV0";
        public const string Level1Qualification = "LV1";
        public const string Level2Qualification = "LV2";
        public const string Level3Qualification = "LV3";
        public const string Level4Qualification = "LV4";
        public const string Level5Qualification = "LV5";
        public const string Level6Qualification = "LV6";
        public const string Level7Qualification = "LV7";
        public const string Level8Qualification = "LV8";
        public const string Level9Qualification = "LV9";
        public const string LevelUnknown = "LVNA";
    }
}