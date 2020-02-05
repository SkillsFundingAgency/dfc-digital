using AutoMapper;
using DFC.Digital.Data.Model;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal class CourseHoursConverter : IValueConverter<FAC.CourseHours, CourseHours>
    {
        public Data.Model.CourseHours Convert(FAC.CourseHours sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case FAC.CourseHours.Fulltime:
                    return Data.Model.CourseHours.Fulltime;
                case FAC.CourseHours.PartTime:
                    return Data.Model.CourseHours.PartTime;
                case FAC.CourseHours.Flexible:
                    return Data.Model.CourseHours.Flexible;
                case FAC.CourseHours.All:
                default:
                    return Data.Model.CourseHours.All;
            }
        }
    }
}