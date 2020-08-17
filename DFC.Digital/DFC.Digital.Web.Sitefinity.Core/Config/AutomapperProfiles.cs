using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<ServiceStatus, ServiceStatusModel>()
            .ForMember(d => d.StatusText, o => o.ConvertUsing(new ServiceHealthStatusConverter(), src => src.Status));

            CreateMap<ServiceStatusChildApp, ServiceStatusChildModel>()
            .ForMember(d => d.StatusText, o => o.ConvertUsing(new ServiceHealthStatusConverter(), src => src.Status));
        }
    }
}