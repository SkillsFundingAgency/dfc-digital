using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.Mapper
{
    public class SkillsFrameworkMapper :Profile
    {
        public SkillsFrameworkMapper()
        {
            CreateMap<DfcGdsAttributesData, sp_GetAttributesByOnNetCodeForEachSectionGroupedV2_Result>()
                .ForMember(s => s.Attribute, m => m.MapFrom(d => d.Attribute))
                .ForMember(s => s.description, m => m.MapFrom(d => d.ElementDescription))
                .ForMember(s => s.total_value, m => m.MapFrom(d => d.Value))
                .ForMember(s => s.element_id, m => m.MapFrom(d => d.ElementId))
                .ForMember(s => s.element_name, m => m.MapFrom(d => d.ElementName));

            CreateMap<DFC_GDSTranlations, DfcGdsTranslation>()
                .ForMember(d => d.DateTimeStamp, m => m.MapFrom(s => s.datetimestamp))
                .ForMember(d => d.Translation, m => m.MapFrom(s => s.translation))
                .ForMember(d => d.ElementId, m => m.MapFrom(s => s.onet_element_id))
                .ReverseMap()
                .ForMember(s=>s.onet_element_id,m=>m.MapFrom(d=>d.ElementId))
                .ForMember(s=>s.datetimestamp,m=>m.MapFrom(d=>d.DateTimeStamp))
                .ForMember(s=>s.translation,m=>m.MapFrom(d=>d.Translation));

            CreateMap<DFC_SocMappings, DfcGdsSocMappings>()
                .ForMember(d => d.JobProfile, m => m.MapFrom(s => s.JobProfile))
                .ForMember(d => d.QualityRating, m => m.MapFrom(s => s.QualityRating))
                .ForMember(d => d.SocCode, m => m.MapFrom(s => s.SocCode))
                .ForMember(d => d.OnetSocCode, m => m.MapFrom(s => s.ONetCode))
                .ReverseMap()
                .ForMember(s => s.SocCode, m => m.MapFrom(d => d.SocCode))
                .ForMember(s => s.JobProfile, m => m.MapFrom(d => d.JobProfile))
                .ForMember(s => s.ONetCode, m => m.MapFrom(d => d.OnetSocCode))
                .ForMember(s => s.QualityRating, m => m.MapFrom(d => d.QualityRating));

        }
        public override string ProfileName => this.GetType().Name;
    }
}
