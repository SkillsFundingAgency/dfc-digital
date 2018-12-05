using AutoMapper;
using DFC.Digital.Data.Model;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    public class DfcSearchModuleAutomapperProfile : Profile
    {
        public DfcSearchModuleAutomapperProfile()
        {
            CreateMap<JobProfileIndex, JobProfileIndex>();
            CreateMap<JobProfileOverloadSearchExtended, JobProfileIndex>()
                .ForMember(m => m.AlternativeTitle, ops => ops.MapFrom(src => string.IsNullOrWhiteSpace(src.AlternativeTitle) ? Enumerable.Empty<string>() : src.AlternativeTitle.Split(',')))
                .ForMember(m => m.Interests, ops => ops.MapFrom(src => src.RelatedInterests))
                .ForMember(m => m.JobAreas, ops => ops.MapFrom(src => src.RelatedJobAreas))
                .ForMember(m => m.PreferredTaskTypes, ops => ops.MapFrom(src => src.RelatedPreferredTaskTypes))
                .ForMember(m => m.TrainingRoutes, ops => ops.MapFrom(src => src.RelatedTrainingRoutes))
                .ForMember(m => m.EntryQualifications, ops => ops.MapFrom(src => src.RelatedEntryQualifications))
                .ForMember(m => m.Enablers, ops => ops.MapFrom(src => src.RelatedInterests))
                .ForMember(m => m.JobProfileCategoriesWithUrl, ops => ops.MapFrom(src => src.JobProfileCategoryIdCollection))
                .ForMember(m => m.IdentityField, ops => ops.MapFrom(src => src.Id.ToString()));
        }

        public override string ProfileName => "DFC.Digital.Web.Sitefinity.DfcSearchModule";
    }
}
