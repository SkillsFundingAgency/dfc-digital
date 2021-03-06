﻿using AutoMapper;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule
{
    public class ContactUsAutomapperProfile : Profile
    {
        public ContactUsAutomapperProfile()
        {
            CreateMap<GeneralFeedbackViewModel, ContactUs>().ForMember(d => d.GeneralFeedback, m => m.MapFrom(s => s));
            CreateMap<TechnicalFeedbackViewModel, ContactUs>().ForMember(d => d.TechnicalFeedback, m => m.MapFrom(s => s));
            CreateMap<ContactAdviserViewModel, ContactUs>().ForMember(d => d.ContactAnAdviserFeedback, m => m.MapFrom(s => s));
            CreateMap<ContactOptionsViewModel, ContactUs>().ForMember(d => d.ContactUsOption, m => m.MapFrom(s => s));
        }
    }
}