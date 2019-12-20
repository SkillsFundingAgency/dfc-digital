using AutoMapper;
using DFC.Digital.Data.Model;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal class CourseTypeConverter : IValueConverter<FAC.CourseType, CourseType>
    {
        public Data.Model.CourseType Convert(FAC.CourseType sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case FAC.CourseType.ClassroomBased:
                    return Data.Model.CourseType.ClassroomBased;
                case FAC.CourseType.Online:
                    return Data.Model.CourseType.Online;
                case FAC.CourseType.WorkBased:
                    return Data.Model.CourseType.WorkBased;
                case FAC.CourseType.All:
                default:
                    return Data.Model.CourseType.All;
            }
        }
    }
}