using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.Mapper
{
    public class SkillsFrameworkMapper : Profile
    {
        public SkillsFrameworkMapper()
        {

                 CreateMap<DFC_GDSTranlations, WhatItTakesSkill>()
                .ForMember(d => d.Description, m => m.MapFrom(s => s.translation.Trim()))
                .ForMember(d => d.Title, m => m.MapFrom(d => d.onet_element_id))
                .ReverseMap()
                .ForMember(s => s.translation, m => m.MapFrom(d => d.Description))
                .ForMember(s => s.onet_element_id, m => m.MapFrom(d => d.Title));

                 CreateMap<DFC_GDSTranlations, FrameworkSkill>()
                .ForMember(d => d.Description, m => m.MapFrom(s => s.translation.Trim()))
                .ForMember(d => d.ONetElementId, m => m.MapFrom(d => d.onet_element_id))
                .ReverseMap()
                .ForMember(s => s.translation, m => m.MapFrom(d => d.Description))
                .ForMember(s => s.onet_element_id, m => m.MapFrom(d => d.ONetElementId));

                CreateMap<DFC_SocMappings, SocCode>()
                .ForMember(d => d.ONetOccupationalCode, m => m.MapFrom(s => s.ONetCode))
                .ForMember(d => d.SOCCode, m => m.MapFrom(s => s.SocCode))
                .ReverseMap()
                .ForMember(d => d.ONetCode, m => m.MapFrom(s => s.ONetOccupationalCode))
                .ForMember(d => d.SocCode, m => m.MapFrom(s => s.SOCCode))
            ;
        }

        public override string ProfileName => "DFC.Digital.Repository.ONET.Mapper";
    }
}
