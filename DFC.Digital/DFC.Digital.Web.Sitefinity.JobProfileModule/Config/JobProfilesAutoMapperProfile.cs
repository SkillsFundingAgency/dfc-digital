using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Globalization;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Config
{
    public class JobProfilesAutoMapperProfile : Profile
    {
        public JobProfilesAutoMapperProfile()
        {
            CreateMap<JobProfileIndex, JobProfile>()
                .ForMember(d => d.AlternativeTitle, o => o.MapFrom(s => string.Join(", ", s.AlternativeTitle).Trim().TrimEnd(',')));

            CreateMap<SearchResultItem<JobProfileIndex>, JobProfileSearchResultItemViewModel>()
                .ForMember(d => d.ResultItemAlternativeTitle, o => o.MapFrom(s => string.Join(", ", s.ResultItem.AlternativeTitle).Trim().TrimEnd(',')))
                .ForMember(c => c.JobProfileCategoriesWithUrl, m => m.MapFrom(j => j.ResultItem.JobProfileCategoriesWithUrl))
                .ForMember(d => d.ResultItemSalaryRange, o => o.MapFrom(s => s.ResultItem.SalaryStarter.Equals(0) || s.ResultItem.SalaryExperienced.Equals(0) ? string.Empty : string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", s.ResultItem.SalaryStarter, s.ResultItem.SalaryExperienced)));

            CreateMap<JobProfile, JobProfileDetailsViewModel>()
                .ForMember(d => d.MinimumHours, o => o.MapFrom(s => (s.MinimumHours != null) ? s.MinimumHours.Value.ToString("#.#") : string.Empty))
                .ForMember(d => d.MaximumHours, o => o.MapFrom(s => (s.MaximumHours != null) ? s.MaximumHours.Value.ToString("#.#") : string.Empty));

            CreateMap<JobProfileSection, AnchorLink>()
                .ForMember(d => d.LinkText, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.LinkTarget, o => o.MapFrom(s => s.ContentField))
                ;

            CreateMap<PsfModel, PreSearchFiltersResultsModel>();
            CreateMap<PsfSection, FilterResultsSection>();
            CreateMap<PsfOption, FilterResultsOption>();

            CreateMap<PsfSection, PreSearchFilterSection>();
            CreateMap<PsfOption, PreSearchFilterOption>();
            CreateMap<PreSearchFilterSection, PsfSection>();
            CreateMap<PreSearchFilterOption, PsfOption>();
        }

        public override string ProfileName => "DFC.Digital.Web.Sitefinity.JobProfileModule";
    }
}