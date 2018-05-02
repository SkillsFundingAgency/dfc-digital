using AutoMapper;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.Import
{
    public class JobProfileImportAutoMapperProfile : Profile
    {
        public JobProfileImportAutoMapperProfile()
        {
            CreateMap<LegacyJobProfile, JobProfile>()
                .ForMember(d => d.AlternativeTitle, o => o.MapFrom(s => s.AlternativeTitles))
                .ForMember(d => d.Salary, o => o.MapFrom(s => s.SalaryDescription))
                .ForMember(d => d.WhatYouWillDo, o => o.MapFrom(s => s.WhatYoullDo))
                .ForMember(d => d.CourseKeywords, o => o.MapFrom(s => s.CourseKeywords))
                ;
        }

        public override string ProfileName => nameof(DFC.Digital.Service.Import.JobProfileImportAutoMapperProfile);
    }
}