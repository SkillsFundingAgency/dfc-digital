using AutoMapper;
using DFC.Digital.Data.Model;
using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal class CourseHoursConverter : IValueConverter<FAC.Enums.CourseHours, CourseHours>
    {
        public Data.Model.CourseHours Convert(FAC.Enums.CourseHours sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case FAC.Enums.CourseHours.Fulltime:
                    return Data.Model.CourseHours.Fulltime;
                case FAC.Enums.CourseHours.PartTime:
                    return Data.Model.CourseHours.PartTime;
                case FAC.Enums.CourseHours.Flexible:
                    return Data.Model.CourseHours.Flexible;
                case FAC.Enums.CourseHours.All:
                default:
                    return Data.Model.CourseHours.All;
            }
        }
    }
}