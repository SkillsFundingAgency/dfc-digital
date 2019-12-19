using AutoMapper;
using DFC.Digital.Data.Model;
using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal class CourseTypeConverter : IValueConverter<FAC.Enums.CourseType, CourseType>
    {
        public Data.Model.CourseType Convert(FAC.Enums.CourseType sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case FAC.Enums.CourseType.ClassroomBased:
                    return Data.Model.CourseType.ClassroomBased;
                case FAC.Enums.CourseType.Online:
                    return Data.Model.CourseType.Online;
                case FAC.Enums.CourseType.WorkBased:
                    return Data.Model.CourseType.WorkBased;
                case FAC.Enums.CourseType.All:
                default:
                    return Data.Model.CourseType.All;
            }
        }
    }
}