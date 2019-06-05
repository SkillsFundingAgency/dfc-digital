using AutoMapper;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Config
{
    public class CourseAutomapperProfile : Profile
    {
        public CourseAutomapperProfile()
        {
            CreateMap<CourseSearchFilters, CourseFiltersViewModel>();
        }
    }
}