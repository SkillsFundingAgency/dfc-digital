using AutoMapper;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

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
            CreateMap<FAC.CourseDetails, CourseDetails>();
            CreateMap<FAC.Course, Course>(); //You need to construct course link as well
            CreateMap<FAC.CourseSearchResult, CourseSearchResult>();
            CreateMap<FAC.CourseSearchResultProperties, CourseSearchResultProperties>();
            CreateMap<CourseSearchProperties, FAC.CourseSearchResult>();
        }
    }
}
