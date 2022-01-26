using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Data.Model.OrchardCore.Uniform;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Repository.SitefinityCMS.OrchardCore;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
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
        private static readonly MappingRepository MappingToolRepository = new MappingRepository();

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
                    model.Uniforms = GetUniforms();
                    count = model.Uniforms.Count;
                    break;
                case ItemTypes.ApprenticeshipLink:
                    model.ApprenticeshipLinks = GetApprenticeshipLinks();
                    count = model.ApprenticeshipLinks.Count;
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

        #region Private Methods - DynamicContentTypes

        private List<OcUniform> GetUniforms()
        {
            var uniforms = dynamicModuleRepository.GetAllUniforms().ToList();

            var jsonData = JsonConvert.SerializeObject(uniforms);

            foreach (var uniform in uniforms)
            {
                MappingToolRepository.InsertMigrationMapping(uniform.SitefinityId, uniform.ContentItemId, ItemTypes.Uniform);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-02-" + ItemTypes.Uniform + "-" + uniforms.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return uniforms;
        }

        private List<OcApprenticeshipLink> GetApprenticeshipLinks()
        {
            var apprenticeshipLinks = dynamicModuleRepository.GetAllApprenticeshipLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(apprenticeshipLinks);

            foreach (var apprenticeshipLink in apprenticeshipLinks)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipLink.SitefinityId, apprenticeshipLink.ContentItemId, ItemTypes.ApprenticeshipLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-03-" + ItemTypes.ApprenticeshipLink + "-" + apprenticeshipLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipLinks;
        }

        #endregion Private Methods - DynamicContentTypes

        #region Private Methods - Classifications

        private IEnumerable<FlatTaxaItem> GetFlatTaxaItems(string flatTaxaName)
        {
            //var sitefinityId = new Guid("C8173F8A-4C28-4D63-9665-3F74997E063D");
            //var migrationMappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId);
            return flatTaxonomyRepository.GetMany(category => category.Taxonomy.Name == flatTaxaName).Select(category => new FlatTaxaItem
            {
                Id = category.Id,
                Name = category.Name,
                Title = category.Title,
                Description = category.Description,
                Url = category.UrlName
            });
        }

        private IEnumerable<ApprenticeshipEntryRequirement> GetApprenticeshipEntryRequirements(string contentType)
        {
            var apprenticeshipEntryRequirements = flatTaxonomyRepository.GetMany(aer => aer.Taxonomy.Name == contentType).Select(aer => new ApprenticeshipEntryRequirement
            {
                SitefinityId = aer.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.ApprenticeshipEntryRequirements,
                DisplayText = aer.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = aer.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = aer.Title },
                TitlePart = new Titlepart() { Title = aer.Title },
                ApprenticeshipEntryRequirements = new Apprenticeshipentryrequirements() { Description = new DFC.Digital.Data.Model.OrchardCore.DescriptionText.Description() { Text = aer.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshipentryrequirements/{aer.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(apprenticeshipEntryRequirements);

            foreach (var apprenticeshipEntryRequirement in apprenticeshipEntryRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipEntryRequirement.SitefinityId, apprenticeshipEntryRequirement.ContentItemId, ItemTypes.ApprenticeshipEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-XX-" + ItemTypes.ApprenticeshipEntryRequirements + "-" + apprenticeshipEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipEntryRequirements;
        }

        #endregion Private Methods - Classifications
    }
}