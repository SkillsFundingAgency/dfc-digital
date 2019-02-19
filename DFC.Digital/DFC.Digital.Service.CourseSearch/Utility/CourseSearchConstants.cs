namespace DFC.Digital.Service.CourseSearchProvider
{
    public static class CourseSearchConstants
    {
        /// <summary>
        /// All attendance modes
        /// </summary>
        public const string AllAttendanceModes = "AM1,AM2,AM3,AM4,AM5,AM6,AM7,AM8,AM9";

        /// <summary>
        /// The class attendance modes
        /// </summary>
        public const string ClassAttendanceModes = "AM1,AM2,AM4,AM9";

        /// <summary>
        /// The online attendance modes
        /// </summary>
        public const string OnlineAttendanceModes = "AM4,AM7,AM8,AM9";

        /// <summary>
        /// The distant attendance modes
        /// </summary>
        public const string DistantAttendanceModes = "AM4,AM5,AM6,AM9";

        /// <summary>
        /// The work attendance modes
        /// </summary>
        public const string WorkAttendanceModes = "AM3,AM4,AM6,AM7,AM9";

        public const string FullTimeStudyMode = "sm1";

        public const string PartTimeStudyMode = "sm2";

        public const string FlexibleStudyMode = "sm4";
    }
}