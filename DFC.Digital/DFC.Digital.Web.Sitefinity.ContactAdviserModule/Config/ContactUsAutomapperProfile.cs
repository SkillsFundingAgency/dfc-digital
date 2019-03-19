using AutoMapper;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Config
{
    public class ContactUsAutomapperProfile : Profile
    {
        public ContactUsAutomapperProfile()
        {
            CreateMap<GeneralFeedbackViewModel, ContactUs>().ForMember(d => d.GeneralFeedback, m => m.MapFrom(s => s.GeneralFeedback));
            CreateMap<TechnicalFeedbackViewModel, ContactUs>().ForMember(d => d.TechnicalFeedback, m => m.MapFrom(s => s.TechnicalFeedback));
            CreateMap<ContactAdviserViewModel, ContactUs>().ForMember(d => d.ContactAnAdviserFeedback, m => m.MapFrom(s => s.ContactAnAdviserFeedback));
        }
    }
}