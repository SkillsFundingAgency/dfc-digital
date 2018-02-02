using Autofac;
using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.DfcSearchModule.Service;
using System;
using System.Linq;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Search.Configuration;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Search;
using Telerik.Sitefinity.Services.Search.Configuration;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    public class SearchModuleInstaller
    {
        public static void PreApplicationStart()
        {
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As dont want to catch General Exception")]
        internal static void RegisterSearchService(string typeName, params object[] constructorArgs)
        {
            try
            {
                var serviceType = TypeResolutionService.ResolveType(typeName, false);
                if (serviceType != null)
                {
                    var service = Activator.CreateInstance(serviceType, constructorArgs);
                    if (service != null)
                    {
                        ServiceBus.UnregisterService<ISearchService>();
                        ServiceBus.RegisterService<ISearchService>(service);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.InnerException.Message);
            }
        }

        private static void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            if (Bootstrapper.IsDataInitialized)
            {
                if (SystemManager.ApplicationModules.Any(p => p.Key == SearchModule.ModuleName))
                {
                    var typeName = typeof(DfcSearchService).FullName;
                    App.WorkWith()
                       .Module(SearchModule.ModuleName)
                       .Initialize()
                       .Localization<DfcSearchResource>();

                    //Cant resolve the ISearchService directly from autofac as sitefinity needs emtpy constructor for boot-up and we had to wire up later with other arguments
                    var autofacContainer = ObjectFactory.Container.Resolve<ILifetimeScope>();
                    var jobprofileSearchService = autofacContainer.Resolve<ISearchService<JobProfileIndex>>();
                    var jobprofileIndexConfig = autofacContainer.Resolve<ISearchIndexConfig>();
                    var jobProfileIndexEnhancer = autofacContainer.Resolve<IJobProfileIndexEnhancer>();
                    var asyncHelper = autofacContainer.Resolve<IAsyncHelper>();
                    var mapper = new MapperConfiguration(c => c.CreateMap<JobProfileIndex, JobProfileIndex>()).CreateMapper();
                    RegisterSearchService(typeName, jobprofileSearchService, jobprofileIndexConfig, jobProfileIndexEnhancer, asyncHelper, mapper);
                    AddSearchServiceConfig(typeName);
                }
            }
        }

        private static void AddSearchServiceConfig(string typeName)
        {
            ConfigManager manager = ConfigManager.GetManager();
            var searchConfig = manager.GetSection<SearchConfig>();

            if (!searchConfig.SearchServices.ContainsKey(DfcSearchService.Name))
            {
                searchConfig.SearchServices.Add(new SearchServiceSettings(searchConfig.SearchServices)
                {
                    Name = DfcSearchService.Name,
                    Title = DfcSearchService.Name,
                    TypeName = typeName,
                    ResourceClassId = "DfcSearchServiceResources",
                });

                using (ElevatedConfigModeRegion config = new ElevatedConfigModeRegion())
                {
                    manager.SaveSection(searchConfig);
                }
            }
        }
    }
}