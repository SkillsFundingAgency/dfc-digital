using AutoMapper;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseSearchApiAutomapperProfile : Profile
    {
        public CourseSearchApiAutomapperProfile()
        {
            CreateMap<FAC.Venue, Venue>();
            CreateMap<FAC.ProviderDetails, ProviderDetails>();
            CreateMap<FAC.Oppurtunity, Oppurtunity>();
            CreateMap<FAC.LocationDetails, LocationDetails>();
            CreateMap<FAC.Address, Address>();
            CreateMap<FAC.CourseSearchFilters, CourseSearchFilters>()
                .ForMember(d => d.CourseHours, o => o.ConvertUsing(new CourseHoursConverter()))
                .ForMember(d => d.CourseType, o => o.ConvertUsing(new CourseTypeConverter()))
                .ReverseMap()
                ;

            CreateMap<FAC.Venue, Venue>();

            CreateMap<FAC.CourseDetails, CourseDetails>()
                .ForSourceMember(s => s.SubRegions, o => o.DoNotValidate())
                .ForMember(c => c.Cost, d => d.ConvertUsing(new CourseCostConverter()))
                .ForMember(d => d.CourseRegions, o => o.ConvertUsing(new CourseRegionsConverter(), src => src.SubRegions));

            CreateMap<FAC.Course, Course>();
            CreateMap<FAC.CourseSearchResult, CourseSearchResult>();
            CreateMap<FAC.CourseSearchResultProperties, CourseSearchResultProperties>();
            CreateMap<CourseSearchProperties, FAC.CourseSearchProperties>();
        }
    }
}
