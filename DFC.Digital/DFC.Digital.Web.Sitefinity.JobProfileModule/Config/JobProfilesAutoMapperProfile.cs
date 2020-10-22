using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Globalization;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
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
                .ForMember(d => d.ResultItemSalaryRange, o => o.MapFrom(s => s.ResultItem.SalaryStarter.Equals(0) || s.ResultItem.SalaryExperienced.Equals(0) ? string.Empty : string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", s.ResultItem.SalaryStarter, s.ResultItem.SalaryExperienced)))
                .ForMember(d => d.ShouldDisplayCaveat, o => o.Condition((a, b, c, d, ctx) =>
                {
                    if (ctx.Items.ContainsKey(nameof(PsfSearchController.CaveatFinderIndexFieldName))
                        && ctx.Items.ContainsKey(nameof(PsfSearchController.CaveatFinderIndexValue))
                        && !string.IsNullOrEmpty(ctx.Items[nameof(PsfSearchController.CaveatFinderIndexFieldName)]?.ToString()))
                    {
                        var indexFieldName = ctx.Items[nameof(PsfSearchController.CaveatFinderIndexFieldName)]?.ToString();
                        var indexField = a.ResultItem.GetType().GetProperties().FirstOrDefault(m => m.Name.Equals(indexFieldName, System.StringComparison.OrdinalIgnoreCase));
                        var indexedValue = indexField.GetValue(a.ResultItem)?.ToString();
                        var expectedCaveatValue = ctx.Items[nameof(PsfSearchController.CaveatFinderIndexValue)]?.ToString();
                        return expectedCaveatValue != null && expectedCaveatValue.Equals(indexedValue, System.StringComparison.OrdinalIgnoreCase);
                    }

                    return false;
                }));

            CreateMap<JobProfile, JobProfileDetailsViewModel>()
                .ForMember(d => d.MinimumHours, o => o.MapFrom(s => (s.MinimumHours != null) ? s.MinimumHours.Value.ToString("#.#") : string.Empty))
                .ForMember(d => d.MaximumHours, o => o.MapFrom(s => (s.MaximumHours != null) ? s.MaximumHours.Value.ToString("#.#") : string.Empty));

            CreateMap<JobProfileSection, AnchorLink>()
                .ForMember(d => d.LinkText, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.LinkTarget, o => o.MapFrom(s => s.ContentField))
                ;
            CreateMap<JobProfileHowToBecomeController, JobProfileHowToBecomeViewModel>();
            CreateMap<PsfModel, PreSearchFiltersResultsModel>();
            CreateMap<PsfSection, FilterResultsSection>();
            CreateMap<PsfOption, FilterResultsOption>();

            CreateMap<PsfSection, PreSearchFilterSection>();
            CreateMap<PsfOption, PreSearchFilterOption>();
            CreateMap<PreSearchFilterSection, PsfSection>();
            CreateMap<PreSearchFilterOption, PsfOption>();

            CreateMap<StructuredDataInjection, JobProfileStructuredDataViewModel>();
        }

        public override string ProfileName => "DFC.Digital.Web.Sitefinity.JobProfileModule";
    }
}