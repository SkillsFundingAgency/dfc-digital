using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.OrchardCore;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        private static readonly OrchardCoreIdGenerator OrchardCoreIdGenerator = new OrchardCoreIdGenerator();

        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IDynamicModuleRepository<JobProfile> dynamicModuleRepository;
        private readonly IFlatTaxonomyRepository flatTaxonomyRepository;
        private readonly ITaxonomyRepository taxonomyRepository;
        private readonly IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository;

        #endregion Private Fields

        #region Constructors
        public MigrationToolController(
            IJobProfileRepository jobProfileRepository,
            IDynamicModuleRepository<JobProfile> dynamicModuleRepository,
            IFlatTaxonomyRepository flatTaxonomyRepository,
            ITaxonomyRepository taxonomyRepository,
            IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.dynamicModuleRepository = dynamicModuleRepository;
            this.flatTaxonomyRepository = flatTaxonomyRepository;
            this.taxonomyRepository = taxonomyRepository;
            this.dynamicModuleSocCodeRepository = dynamicModuleSocCodeRepository;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Current ContentType Name")]
        public string CurrentContentTypeName { get; set; }

        [DisplayName("Json FilePath")]
        public string JsonFilePath { get; set; } = @"D:\ESFA-GitHub\dfc-digital\DFC.Digital\DFC.Digital.Web.Sitefinity\OrchardCoreRecipes\";

        [DisplayName("Recipe Beginning")]
        public string RecipeBeginning { get; set; } = "{ \"name\": \"\", \"displayName\": \"\", \"description\": \"\", \"author\": \"\", \"website\": \"\", \"version\": \"\", \"issetuprecipe\": false, \"categories\": [], \"tags\": [],\"steps\": [{ \"name\": \"content\", \"data\": ";

        [DisplayName("Recipe End")]
        public string RecipeEnd { get; set; } = "}]}";

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
                    model.Uniforms = dynamicModuleRepository.GetAllUniforms().ToList();
                    count = model.Uniforms.Count;
                    break;
                case ItemTypes.HiddenAlternativeTitle:
                    model.FlatTaxaItems = GetFlatTaxaItems(ItemTypes.HiddenAlternativeTitle).ToList();
                    count = model.FlatTaxaItems.Count;
                    break;
                case ItemTypes.ApprenticeshipEntryRequirements:
                    model.ApprenticeshipEntryRequirements = GetApprenticeshipEntryRequirements(ItemTypes.ApprenticeshipEntryRequirements).ToList();
                    count = model.ApprenticeshipEntryRequirements.Count;
                    break;
                default:
                    break;
            }

            //var socCode = dynamicModuleSocCodeRepository.GetAll().ToList();
            model.Message = $"We have {count} items of '{model.SelectedItemType}' content type.";

            return View("~/Frontend-Assembly/DFC.Digital.Web.Sitefinity.JobProfileModule/Mvc/Views/MigrationTool/index.cshtml", model);
        }

        #endregion Actions

        #region Private Methods

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

        private IEnumerable<ApprenticeshipEntryRequirement> GetApprenticeshipEntryRequirements(string flatTaxaName)
        {
            var apprenticeshipEntryRequirements = flatTaxonomyRepository.GetMany(aer => aer.Taxonomy.Name == flatTaxaName).Select(aer => new ApprenticeshipEntryRequirement
            {
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.ApprenticeshipEntryRequirements,
                DisplayText = aer.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = aer.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = aer.Title },
                TitlePart = new Titlepart() { Title = aer.Title },
                ApprenticeshipEntryRequirements = new Apprenticeshipentryrequirements() { Description = new Description() { Text = aer.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshipentryrequirements/{aer.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(apprenticeshipEntryRequirements);

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-" + apprenticeshipEntryRequirements.Count().ToString() + "-" + ItemTypes.ApprenticeshipEntryRequirements + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipEntryRequirements;
        }

        #endregion Private Methods
    }
}