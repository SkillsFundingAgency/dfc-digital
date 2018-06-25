using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Mapper
{
    using AutoMapper;
    using Data.Model;
    using DataModel;

    class SkillsFrameworkMapper :Profile
    {
        public SkillsFrameworkMapper()
        {
            CreateMap<AttributesData, sp_GetAttributesByOnNetCodeForEachSectionGroupedV2_Result>()
                .ForMember(s => s.Attribute, m => m.MapFrom(d => d.Attribute))
                .ForMember(s => s.description, m => m.MapFrom(d => d.ElementDescription))
                .ForMember(s => s.total_value, m => m.MapFrom(d => d.Value))
                .ForMember(s => s.element_id, m => m.MapFrom(d => d.ElementId))
                .ForMember(s => s.element_name, m => m.MapFrom(d => d.ElementName));
        }
    }
}
