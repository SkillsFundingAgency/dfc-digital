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
            CreateMap<ContactUsViewModel, ContactUsViewModel>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}