using AutoMapper;
using DFC.Digital.Data.Model;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid.Config
{
    public class SendGridAutoMapperProfile : Profile
    {
        public SendGridAutoMapperProfile()
        {
            CreateMap<Response, SendEmailResponse>();
        }
    }
}
