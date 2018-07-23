using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.Mapper
{
    public class SkillsFrameworkMapper :Profile
    {
        public SkillsFrameworkMapper()
        {
            CreateMap<OnetAttribute, sp_GetAttributesByOnNetCodeForEachSectionGroupedV2_Result>()
                .ForMember(s => s.Attribute, m => m.MapFrom(d => d.Attribute))
                .ForMember(s => s.description, m => m.MapFrom(d => d.Description))
                .ForMember(s => s.total_value, m => m.MapFrom(d => d.Score))
                .ForMember(s => s.element_id, m => m.MapFrom(d => d.Id))
                .ForMember(s => s.element_name, m => m.MapFrom(d => d.Name));

            CreateMap<DFC_GDSTranlations, DfcOnetTranslation>()
                .ForMember(d => d.DateTimeStamp, m => m.MapFrom(s => s.datetimestamp))
                .ForMember(d => d.Translation, m => m.MapFrom(s => s.translation))
                .ForMember(d => d.ElementId, m => m.MapFrom(s => s.onet_element_id))
                .ReverseMap()
                .ForMember(s=>s.onet_element_id,m=>m.MapFrom(d=>d.ElementId))
                .ForMember(s=>s.datetimestamp,m=>m.MapFrom(d=>d.DateTimeStamp))
                .ForMember(s=>s.translation,m=>m.MapFrom(d=>d.Translation));

            CreateMap<DFC_SocMappings, SocCode>()
                .ForMember(d => d.ONetOccupationalCode, m => m.MapFrom(s => s.ONetCode))
                .ForMember(d => d.SOCCode, m => m.MapFrom(s => s.SocCode));
        }

        public override string ProfileName => this.GetType().Name;
    }
}
