using System.Globalization;
using AutoMapper;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class CmsExtensionsAutoMapperProfile : Profile
    {
        public CmsExtensionsAutoMapperProfile()
        {
            CreateMap<CmsReportItem, JobProfileReport>();
            CreateMap<CmsReportItem, ApprenticeshipVacancyReport>();
            CreateMap<CmsReportItem, SocCodeReport>();
        }

        public override string ProfileName => "DFC.Digital.Web.Sitefinity.JobProfileModule";
    }
}