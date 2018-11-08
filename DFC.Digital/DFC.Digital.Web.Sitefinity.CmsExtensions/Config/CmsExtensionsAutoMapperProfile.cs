using AutoMapper;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class CmsExtensionsAutoMapperProfile : Profile
    {
        public CmsExtensionsAutoMapperProfile()
        {
            CreateMap<CmsReportItem, JobProfileReport>()
                .ForMember(d => d.Name, c => c.MapFrom(s => s.UrlName));

            CreateMap<CmsReportItem, ApprenticeshipVacancyReport>()
                .ForMember(d => d.Name, c => c.MapFrom(s => s.UrlName));

            CreateMap<CmsReportItem, SocCodeReport>();
        }

        public override string ProfileName => "DFC.Digital.Web.Sitefinity.JobProfileModule";
    }
}