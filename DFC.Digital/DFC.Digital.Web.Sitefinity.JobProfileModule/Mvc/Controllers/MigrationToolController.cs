using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "MigrationTool", Title = "Migration Tool", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class MigrationToolController : Controller
    {
        #region Private Fields

        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IDynamicModuleRepository<JobProfile> dynamicModuleRepository;
        private readonly IDynamicModuleRepository<Uniform> dynamicModuleUniformRepository;
        private readonly IFlatTaxonomyRepository flatTaxonomyRepository;
        private readonly ITaxonomyRepository taxonomyRepository;
        private readonly IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository;

        #endregion Private Fields

        #region Constructors
        public MigrationToolController(
            IJobProfileRepository jobProfileRepository,
            IDynamicModuleRepository<JobProfile> dynamicModuleRepository,
            IDynamicModuleRepository<Uniform> dynamicModuleUniformRepository,
            IFlatTaxonomyRepository flatTaxonomyRepository,
            ITaxonomyRepository taxonomyRepository,
            IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.dynamicModuleRepository = dynamicModuleRepository;
            this.dynamicModuleUniformRepository = dynamicModuleUniformRepository;
            this.flatTaxonomyRepository = flatTaxonomyRepository;
            this.taxonomyRepository = taxonomyRepository;
            this.dynamicModuleSocCodeRepository = dynamicModuleSocCodeRepository;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Current ContentType Name")]
        public string CurrentContentTypeName { get; set; }

        #endregion Public Properties

        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MigrationToolViewModel();
            model.Message = "Please, select ContentType for migration";

            return View(model);
        }

        [HttpPost]
        public ActionResult ContentItems(MigrationToolViewModel model)
        {
            int count = 0;

            switch (model.SelectedItemType)
            {
                case ItemTypes.JobProfile:
                    model.JobProfiles = jobProfileRepository.GetAllJobProfiles().ToList();
                    count = model.JobProfiles.Count;
                    break;
                case ItemTypes.Uniform:
                    model.Uniforms = dynamicModuleUniformRepository.GetAllUniforms().ToList();
                    count = model.Uniforms.Count;
                    break;
                case ItemTypes.HiddenAlternativeTitle:
                    model.FlatTaxaItems = GetFlatTaxaItems(ItemTypes.HiddenAlternativeTitle).ToList();
                    count = model.FlatTaxaItems.Count;
                    break;
                default:
                    break;
            }

            //var socCode = dynamicModuleSocCodeRepository.GetAll().ToList();
            model.Message = $"We have {count} items of '{model.SelectedItemType}' content type.";

            return View("~/Frontend-Assembly/DFC.Digital.Web.Sitefinity.JobProfileModule/Mvc/Views/MigrationTool/index.cshtml", model);
        }

        private IEnumerable<FlatTaxaItem> GetFlatTaxaItems(string flatTaxaName)
        {
            return flatTaxonomyRepository.GetMany(category => category.Taxonomy.Name == flatTaxaName).Select(category => new FlatTaxaItem
            {
                Id = category.Id,
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Url = category.UrlName
            });
        }

        #endregion Actions
    }
}