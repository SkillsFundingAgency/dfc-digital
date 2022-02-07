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

        #endregion Private Fields

        #region Constructors
        public MigrationToolController(
            IJobProfileRepository jobProfileRepository,
            IDynamicModuleRepository<JobProfile> dynamicModuleRepository,
            IFlatTaxonomyRepository flatTaxonomyRepository,
            ITaxonomyRepository taxonomyRepository)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.dynamicModuleRepository = dynamicModuleRepository;
            this.flatTaxonomyRepository = flatTaxonomyRepository;
            this.taxonomyRepository = taxonomyRepository;
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

        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }

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
                case ItemTypes.ApprenticeshipStandards:
                    model.ApprenticeshipStandards = GetApprenticeshipStandards().ToList();
                    count = model.ApprenticeshipStandards.Count;
                    break;
                case ItemTypes.JobProfileSoc:
                    model.SocCodes = GetSocCodes().ToList();
                    model.ErrorMessage = ErrorMessage;
                    count = model.SocCodes.Count;
                    break;
                case ItemTypes.ApprenticeshipLink:
                    model.ApprenticeshipLinks = GetApprenticeshipLinks();
                    count = model.ApprenticeshipLinks.Count;
                    break;
                case ItemTypes.CollegeLink:
                    model.CollegeLinks = GetCollegeLinks();
                    count = model.CollegeLinks.Count;
                    break;
                case ItemTypes.UniversityLink:
                    model.UniversityLinks = GetUniversityLinks();
                    count = model.UniversityLinks.Count;
                    break;
                case ItemTypes.ApprenticeshipRequirement:
                    model.ApprenticeshipRequirements = GetApprenticeshipRequirements();
                    count = model.ApprenticeshipRequirements.Count;
                    break;
                case ItemTypes.CollegeRequirement:
                    model.CollegeRequirements = GetCollegeRequirements();
                    count = model.CollegeRequirements.Count;
                    break;
                case ItemTypes.UniversityRequirement:
                    model.UniversityRequirements = GetUniversityRequirements();
                    count = model.UniversityRequirements.Count;
                    break;
                case ItemTypes.Uniform:
                    model.Uniforms = GetUniforms();
                    count = model.Uniforms.Count;
                    break;
                case ItemTypes.Location:
                    model.Locations = GetLocations();
                    count = model.Locations.Count;
                    break;
                case ItemTypes.Environment:
                    model.Environments = GetEnvironments();
                    count = model.Environments.Count;
                    break;
                case ItemTypes.Registration:
                    model.Registrations = GetRegistrations();
                    count = model.Registrations.Count;
                    break;
                case ItemTypes.Restriction:
                    model.Restrictions = GetRestrictions();
                    count = model.Restrictions.Count;
                    break;
                case ItemTypes.HiddenAlternativeTitle:
                    //model.FlatTaxaItems = GetFlatTaxaItems(ItemTypes.HiddenAlternativeTitle).ToList();
                    //count = model.FlatTaxaItems.Count;
                    model.HiddenAlternativeTitles = GetHiddenAlternativeTitles().ToList();
                    count = model.HiddenAlternativeTitles.Count;
                    break;
                case ItemTypes.ApprenticeshipEntryRequirements:
                    model.ApprenticeshipEntryRequirements = GetApprenticeshipEntryRequirements().ToList();
                    count = model.ApprenticeshipEntryRequirements.Count;
                    break;
                case ItemTypes.WorkingPattern:
                    model.WorkingPatterns = GetWorkingPatterns().ToList();
                    count = model.WorkingPatterns.Count;
                    break;
                case ItemTypes.JobProfileCategories:
                    model.JobProfileCategories = GetJobProfileCategories().ToList();
                    count = model.JobProfileCategories.Count;
                    break;
                case ItemTypes.UniversityEntryRequirements:
                    model.UniversityEntryRequirements = GetUniversityEntryRequirements().ToList();
                    count = model.UniversityEntryRequirements.Count;
                    break;
                case ItemTypes.CollegeEntryRequirements:
                    model.CollegeEntryRequirements = GetCollegeEntryRequirements().ToList();
                    count = model.CollegeEntryRequirements.Count;
                    break;
                case ItemTypes.JobProfileSpecialism:
                    model.JobProfileSpecialisms = GetJobProfileSpecialisms().ToList();
                    count = model.JobProfileSpecialisms.Count;
                    break;
                case ItemTypes.WorkingPatternDetails:
                    model.WorkingPatternDetails = GetWorkingPatternDetails().ToList();
                    count = model.WorkingPatternDetails.Count;
                    break;
                case ItemTypes.WorkingHoursDetails:
                    model.WorkingHoursDetails = GetWorkingHoursDetails().ToList();
                    count = model.WorkingHoursDetails.Count;
                    break;
                case ItemTypes.JobProfile:
                    //model.JobProfiles = jobProfileRepository.GetAllJobProfiles().ToList();
                    //count = model.JobProfiles.Count;
                    model.JobProfiles = GetJobProfiles();
                    count = model.JobProfiles.Count;
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

        private List<OcRegistration> GetRegistrations()
        {
            var registrations = dynamicModuleRepository.GetAllRegistrations().ToList();

            var jsonData = JsonConvert.SerializeObject(registrations);
            var jsonRegistrations = JsonConvert.DeserializeObject<List<OcRegistration>>(jsonData);

            for (int i = 0; i < jsonRegistrations.Count(); i++)
            {
                jsonRegistrations[i].SitefinityId = registrations[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(registrations[i].SitefinityId, jsonRegistrations[i].ContentItemId, ItemTypes.Registration);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-12-" + ItemTypes.Registration + "-" + registrations.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonRegistrations;
        }

        private List<OcRestriction> GetRestrictions()
        {
            var restrictions = dynamicModuleRepository.GetAllRestrictions().ToList();

            var jsonData = JsonConvert.SerializeObject(restrictions);
            var jsonRestrictions = JsonConvert.DeserializeObject<List<OcRestriction>>(jsonData);

            for (int i = 0; i < jsonRestrictions.Count(); i++)
            {
                jsonRestrictions[i].SitefinityId = restrictions[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(restrictions[i].SitefinityId, jsonRestrictions[i].ContentItemId, ItemTypes.Restriction);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-13-" + ItemTypes.Restriction + "-" + restrictions.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonRestrictions;
        }

        private List<OcApprenticeshipLink> GetApprenticeshipLinks()
        {
            var apprenticeshipLinks = dynamicModuleRepository.GetAllApprenticeshipLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(apprenticeshipLinks);
            var jsonApprenticeshipLinks = JsonConvert.DeserializeObject<List<OcApprenticeshipLink>>(jsonData);

            for (int i = 0; i < jsonApprenticeshipLinks.Count(); i++)
            {
                jsonApprenticeshipLinks[i].SitefinityId = apprenticeshipLinks[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(apprenticeshipLinks[i].SitefinityId, jsonApprenticeshipLinks[i].ContentItemId, ItemTypes.ApprenticeshipLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-03-" + ItemTypes.ApprenticeshipLink + "-" + apprenticeshipLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonApprenticeshipLinks;
        }

        private List<OcCollegeLink> GetCollegeLinks()
        {
            var collegeLinks = dynamicModuleRepository.GetAllCollegeLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(collegeLinks);
            var jsonCollegeLinks = JsonConvert.DeserializeObject<List<OcCollegeLink>>(jsonData);

            for (int i = 0; i < jsonCollegeLinks.Count(); i++)
            {
                jsonCollegeLinks[i].SitefinityId = collegeLinks[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(collegeLinks[i].SitefinityId, jsonCollegeLinks[i].ContentItemId, ItemTypes.CollegeLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-04-" + ItemTypes.CollegeLink + "-" + collegeLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonCollegeLinks;
        }

        private List<OcUniversityLink> GetUniversityLinks()
        {
            var universityLinks = dynamicModuleRepository.GetAllUniversityLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(universityLinks);
            var jsonUniversityLinks = JsonConvert.DeserializeObject<List<OcUniversityLink>>(jsonData);

            for (int i = 0; i < jsonUniversityLinks.Count(); i++)
            {
                jsonUniversityLinks[i].SitefinityId = universityLinks[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(universityLinks[i].SitefinityId, jsonUniversityLinks[i].ContentItemId, ItemTypes.UniversityLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-05-" + ItemTypes.UniversityLink + "-" + universityLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonUniversityLinks;
        }

        private List<OcApprenticeshipRequirement> GetApprenticeshipRequirements()
        {
            var apprenticeshipRequirements = dynamicModuleRepository.GetAllApprenticeshipRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(apprenticeshipRequirements);
            var jsonApprenticeshipRequirements = JsonConvert.DeserializeObject<List<OcApprenticeshipRequirement>>(jsonData);

            for (int i = 0; i < jsonApprenticeshipRequirements.Count(); i++)
            {
                jsonApprenticeshipRequirements[i].SitefinityId = apprenticeshipRequirements[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(apprenticeshipRequirements[i].SitefinityId, jsonApprenticeshipRequirements[i].ContentItemId, ItemTypes.ApprenticeshipRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-06-" + ItemTypes.ApprenticeshipRequirement + "-" + apprenticeshipRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonApprenticeshipRequirements;
        }

        private List<OcCollegeRequirement> GetCollegeRequirements()
        {
            var collegeRequirements = dynamicModuleRepository.GetAllCollegeRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(collegeRequirements);
            var jsonCollegeRequirements = JsonConvert.DeserializeObject<List<OcCollegeRequirement>>(jsonData);

            for (int i = 0; i < jsonCollegeRequirements.Count(); i++)
            {
                jsonCollegeRequirements[i].SitefinityId = collegeRequirements[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(collegeRequirements[i].SitefinityId, jsonCollegeRequirements[i].ContentItemId, ItemTypes.CollegeRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-07-" + ItemTypes.CollegeRequirement + "-" + collegeRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonCollegeRequirements;
        }

        private List<OcUniversityRequirement> GetUniversityRequirements()
        {
            var universityRequirements = dynamicModuleRepository.GetAllUniversityRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(universityRequirements);
            var jsonUniversityRequirements = JsonConvert.DeserializeObject<List<OcUniversityRequirement>>(jsonData);

            for (int i = 0; i < jsonUniversityRequirements.Count(); i++)
            {
                jsonUniversityRequirements[i].SitefinityId = universityRequirements[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(universityRequirements[i].SitefinityId, jsonUniversityRequirements[i].ContentItemId, ItemTypes.UniversityRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-08-" + ItemTypes.UniversityRequirement + "-" + universityRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonUniversityRequirements;
        }

        private List<OcUniform> GetUniforms()
        {
            var uniforms = dynamicModuleRepository.GetAllUniforms().ToList();

            var jsonData = JsonConvert.SerializeObject(uniforms);
            var jsonUniforms = JsonConvert.DeserializeObject<List<OcUniform>>(jsonData);

            for (int i = 0; i < jsonUniforms.Count(); i++)
            {
                jsonUniforms[i].SitefinityId = uniforms[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(uniforms[i].SitefinityId, jsonUniforms[i].ContentItemId, ItemTypes.Uniform);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-09-" + ItemTypes.Uniform + "-" + uniforms.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonUniforms;
        }

        private List<OcLocation> GetLocations()
        {
            var locations = dynamicModuleRepository.GetAllLocations().ToList();

            var jsonData = JsonConvert.SerializeObject(locations);
            var jsonLocations = JsonConvert.DeserializeObject<List<OcLocation>>(jsonData);

            for (int i = 0; i < jsonLocations.Count(); i++)
            {
                jsonLocations[i].SitefinityId = locations[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(locations[i].SitefinityId, jsonLocations[i].ContentItemId, ItemTypes.Location);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-10-" + ItemTypes.Location + "-" + locations.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonLocations;
        }

        private List<OcEnvironment> GetEnvironments()
        {
            var environments = dynamicModuleRepository.GetAllEnvironments().ToList();

            var jsonData = JsonConvert.SerializeObject(environments);
            var jsonEnvironments = JsonConvert.DeserializeObject<List<OcEnvironment>>(jsonData);

            for (int i = 0; i < jsonEnvironments.Count(); i++)
            {
                jsonEnvironments[i].SitefinityId = environments[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(environments[i].SitefinityId, jsonEnvironments[i].ContentItemId, ItemTypes.Environment);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-11-" + ItemTypes.Environment + "-" + environments.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonEnvironments;
        }

        #endregion Private Methods - DynamicContentTypes

        #region Private Methods - Classifications

        /* // GetFlatTaxaItems
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
        */

        private IEnumerable<OcHiddenAlternativeTitle> GetHiddenAlternativeTitles()
        {
            var hiddenAlternativeTitles = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.HiddenAlternativeTitle).Select(ft => new OcHiddenAlternativeTitle
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.HiddenAlternativeTitle,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                HiddenAlternativeTitle = new Hiddenalternativetitle() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/hiddenalternativetitle/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(hiddenAlternativeTitles);
            var jsonHiddenAlternativeTitles = JsonConvert.DeserializeObject<List<OcHiddenAlternativeTitle>>(jsonData);

            for (int i = 0; i < jsonHiddenAlternativeTitles.Count(); i++)
            {
                jsonHiddenAlternativeTitles[i].SitefinityId = hiddenAlternativeTitles.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(hiddenAlternativeTitles.ElementAt(i).SitefinityId, jsonHiddenAlternativeTitles[i].ContentItemId, ItemTypes.HiddenAlternativeTitle);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-14-" + ItemTypes.HiddenAlternativeTitle + "-" + hiddenAlternativeTitles.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonHiddenAlternativeTitles;
        }

        private IEnumerable<ApprenticeshipEntryRequirement> GetApprenticeshipEntryRequirements()
        {
            var apprenticeshipEntryRequirements = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.ApprenticeshipEntryRequirements).Select(ft => new ApprenticeshipEntryRequirement
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.ApprenticeshipEntryRequirements,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                ApprenticeshipEntryRequirements = new Apprenticeshipentryrequirements() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshipentryrequirements/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(apprenticeshipEntryRequirements);
            var jsonApprenticeshipEntryRequirements = JsonConvert.DeserializeObject<List<ApprenticeshipEntryRequirement>>(jsonData);

            for (int i = 0; i < jsonApprenticeshipEntryRequirements.Count(); i++)
            {
                jsonApprenticeshipEntryRequirements[i].SitefinityId = apprenticeshipEntryRequirements.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(apprenticeshipEntryRequirements.ElementAt(i).SitefinityId, jsonApprenticeshipEntryRequirements[i].ContentItemId, ItemTypes.ApprenticeshipEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-15-" + ItemTypes.ApprenticeshipEntryRequirements + "-" + apprenticeshipEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonApprenticeshipEntryRequirements;
        }

        private IEnumerable<OcWorkingPattern> GetWorkingPatterns()
        {
            var workingPatterns = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.WorkingPattern).Select(ft => new OcWorkingPattern
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.WorkingPatterns,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                WorkingPatterns = new Workingpatterns() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/workingpatterns/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(workingPatterns);
            var jsonWorkingPatterns = JsonConvert.DeserializeObject<List<OcWorkingPattern>>(jsonData);

            for (int i = 0; i < jsonWorkingPatterns.Count(); i++)
            {
                jsonWorkingPatterns[i].SitefinityId = workingPatterns.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(workingPatterns.ElementAt(i).SitefinityId, jsonWorkingPatterns[i].ContentItemId, ItemTypes.WorkingPattern);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-16-" + ItemTypes.WorkingPattern + "-" + workingPatterns.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonWorkingPatterns;
        }

        private IEnumerable<OcJobProfileCategory> GetJobProfileCategories()
        {
            var jobProfileCategories = taxonomyRepository.GetMany(ht => ht.Taxonomy.Name == ItemTypes.JobProfileCategories).Select(ht => new OcJobProfileCategory
            {
                SitefinityId = ht.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.JobProfileCategory,
                DisplayText = ht.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ht.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ht.Title },
                TitlePart = new Titlepart() { Title = ht.Title },
                JobProfileCategory = new Jobprofilecategory() { Description = new OcDescriptionText { Text = ht.Description } },
                PageLocationPart = new PagelocationpartJpc() { UrlName = ht.UrlName, DefaultPageForLocation = false, RedirectLocations = null, FullUrl = $"/{ht.UrlName}" },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/jobprofilecategory/{ht.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(jobProfileCategories);
            var jsonJobProfileCategories = JsonConvert.DeserializeObject<List<OcJobProfileCategory>>(jsonData);

            for (int i = 0; i < jsonJobProfileCategories.Count(); i++)
            {
                jsonJobProfileCategories[i].SitefinityId = jobProfileCategories.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(jobProfileCategories.ElementAt(i).SitefinityId, jsonJobProfileCategories[i].ContentItemId, ItemTypes.JobProfileCategories);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-17-" + ItemTypes.JobProfileCategories + "-" + jobProfileCategories.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jobProfileCategories;
        }

        private IEnumerable<OcApprenticeshipStandard> GetApprenticeshipStandards()
        {
            var apprenticeshipStandards = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.ApprenticeshipStandards).Select(ft => new OcApprenticeshipStandard
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.ApprenticeshipStandard,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                ApprenticeshipStandard = new Apprenticeshipstandard() { Description = new OcDescriptionText { Text = ft.Description }, LARScode = new Larscode { Text = ft.UrlName } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshipstandard/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonApprenticeshipStandards = new List<OcApprenticeshipStandard>();
            int allApprenticeshipStandardsCount = apprenticeshipStandards.Count();
            int batchSize = 50;
            int numberOfFiles = allApprenticeshipStandardsCount / batchSize;

            for (int idx = 0; idx <= numberOfFiles; idx++)
            {
                var currentApprenticeshipStandards = apprenticeshipStandards.Skip(idx * batchSize).Take(batchSize).ToList();

                var currentJsonData = JsonConvert.SerializeObject(currentApprenticeshipStandards);
                var jsonCurruntApprenticeshipStandards = JsonConvert.DeserializeObject<List<OcApprenticeshipStandard>>(currentJsonData);

                for (int i = 0; i < jsonCurruntApprenticeshipStandards.Count(); i++)
                {
                    jsonCurruntApprenticeshipStandards[i].SitefinityId = currentApprenticeshipStandards[i].SitefinityId;
                    MappingToolRepository.InsertMigrationMapping(currentApprenticeshipStandards[i].SitefinityId, currentApprenticeshipStandards[i].ContentItemId, ItemTypes.ApprenticeshipStandards);
                }

                var currentFullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-01-" + ItemTypes.ApprenticeshipStandards + $"-{idx + 1}-" + currentApprenticeshipStandards.Count().ToString() + ".json";
                System.IO.File.WriteAllText(currentFullPathAndFileName, RecipeBeginning + currentJsonData + RecipeEnd);

                jsonApprenticeshipStandards.AddRange(jsonCurruntApprenticeshipStandards);
            }

            /*
            var jsonData = JsonConvert.SerializeObject(apprenticeshipStandards);
            var jsonApprenticeshipStandards = JsonConvert.DeserializeObject<List<OcApprenticeshipStandard>>(jsonData);

            for (int i = 0; i < jsonApprenticeshipStandards.Count(); i++)
            {
                jsonApprenticeshipStandards[i].SitefinityId = apprenticeshipStandards.ElementAt(i).SitefinityId;

                //MappingToolRepository.InsertMigrationMapping(apprenticeshipStandards.ElementAt(i).SitefinityId, jsonApprenticeshipStandards[i].ContentItemId, ItemTypes.ApprenticeshipStandards);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-01-" + ItemTypes.ApprenticeshipStandards + "-" + apprenticeshipStandards.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            */

            return jsonApprenticeshipStandards;
        }

        private IEnumerable<OcUniversityEntryRequirement> GetUniversityEntryRequirements()
        {
            var universityEntryRequirements = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.UniversityEntryRequirements).Select(ft => new OcUniversityEntryRequirement
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.UniversityEntryRequirements,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                UniversityEntryRequirements = new Universityentryrequirements() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/universityentryrequirements/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(universityEntryRequirements);
            var jsonUniversityEntryRequirements = JsonConvert.DeserializeObject<List<OcUniversityEntryRequirement>>(jsonData);

            for (int i = 0; i < jsonUniversityEntryRequirements.Count(); i++)
            {
                jsonUniversityEntryRequirements[i].SitefinityId = universityEntryRequirements.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(universityEntryRequirements.ElementAt(i).SitefinityId, jsonUniversityEntryRequirements[i].ContentItemId, ItemTypes.UniversityEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-18-" + ItemTypes.UniversityEntryRequirements + "-" + universityEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonUniversityEntryRequirements;
        }

        private IEnumerable<OcCollegeEntryRequirement> GetCollegeEntryRequirements()
        {
            var collegeEntryRequirements = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.CollegeEntryRequirements).Select(ft => new OcCollegeEntryRequirement
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.CollegeEntryRequirements,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                CollegeEntryRequirements = new Collegeentryrequirements() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/collegeentryrequirements/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(collegeEntryRequirements);
            var jsonCollegeEntryRequirements = JsonConvert.DeserializeObject<List<OcCollegeEntryRequirement>>(jsonData);

            for (int i = 0; i < jsonCollegeEntryRequirements.Count(); i++)
            {
                jsonCollegeEntryRequirements[i].SitefinityId = collegeEntryRequirements.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(collegeEntryRequirements.ElementAt(i).SitefinityId, jsonCollegeEntryRequirements[i].ContentItemId, ItemTypes.CollegeEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-19-" + ItemTypes.CollegeEntryRequirements + "-" + collegeEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonCollegeEntryRequirements;
        }

        private IEnumerable<OcJobProfileSpecialism> GetJobProfileSpecialisms()
        {
            var jobProfileSpecialisms = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.JobProfileSpecialism).Select(ft => new OcJobProfileSpecialism
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.JobProfileSpecialism,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                JobProfileSpecialism = new Jobprofilespecialism() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/jobprofilespecialism/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(jobProfileSpecialisms);
            var jsonJobProfileSpecialism = JsonConvert.DeserializeObject<List<OcJobProfileSpecialism>>(jsonData);

            for (int i = 0; i < jsonJobProfileSpecialism.Count(); i++)
            {
                jsonJobProfileSpecialism[i].SitefinityId = jobProfileSpecialisms.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(jobProfileSpecialisms.ElementAt(i).SitefinityId, jsonJobProfileSpecialism[i].ContentItemId, ItemTypes.JobProfileSpecialism);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-20-" + ItemTypes.JobProfileSpecialism + "-" + jobProfileSpecialisms.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonJobProfileSpecialism;
        }

        private IEnumerable<OcWorkingPatternDetail> GetWorkingPatternDetails()
        {
            var workingPatternDetails = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.WorkingPatternDetails).Select(ft => new OcWorkingPatternDetail
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.WorkingPatternDetail,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                WorkingPatternDetail = new Workingpatterndetail() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/workingpatterndetail/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(workingPatternDetails);
            var jsonWorkingPatternDetails = JsonConvert.DeserializeObject<List<OcWorkingPatternDetail>>(jsonData);

            for (int i = 0; i < jsonWorkingPatternDetails.Count(); i++)
            {
                jsonWorkingPatternDetails[i].SitefinityId = workingPatternDetails.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(workingPatternDetails.ElementAt(i).SitefinityId, jsonWorkingPatternDetails[i].ContentItemId, ItemTypes.WorkingPatternDetails);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-21-" + ItemTypes.WorkingPatternDetails + "-" + workingPatternDetails.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonWorkingPatternDetails;
        }

        private IEnumerable<OcWorkingHoursDetail> GetWorkingHoursDetails()
        {
            var workingHoursDetails = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.WorkingHoursDetails).Select(ft => new OcWorkingHoursDetail
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.WorkingHoursDetail,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                WorkingHoursDetail = new Workinghoursdetail() { Description = new OcDescriptionText { Text = ft.Description } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/workinghoursdetail/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(workingHoursDetails);
            var jsonWorkingHoursDetails = JsonConvert.DeserializeObject<List<OcWorkingHoursDetail>>(jsonData);

            for (int i = 0; i < jsonWorkingHoursDetails.Count(); i++)
            {
                jsonWorkingHoursDetails[i].SitefinityId = workingHoursDetails.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(workingHoursDetails.ElementAt(i).SitefinityId, jsonWorkingHoursDetails[i].ContentItemId, ItemTypes.WorkingHoursDetails);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-22-" + ItemTypes.WorkingHoursDetails + "-" + workingHoursDetails.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonWorkingHoursDetails;
        }

        #endregion Private Methods - Classifications

        #region Private Methods - DynamicContentTypes - SOC Codes and Skills

        private List<OcSocCode> GetSocCodes()
        {
            var socCodes = dynamicModuleRepository.GetAllSOCCodes().ToList();

            foreach (var socCode in socCodes)
            {
                var orchardCoreIds = new List<string>();
                foreach (var sitefinityId in socCode.SOCCode.ApprenticeshipStandardsSitefinityIds.SitefinityIds ?? new List<Guid>())
                {
                    var mappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId).ToList();
                    if (mappings != null || mappings.Count() == 0)
                    {
                        if (mappings.Count() == 1)
                        {
                            orchardCoreIds.Add(mappings[0].OrchardCoreId);
                        }
                        else
                        {
                            orchardCoreIds.Add(mappings.FirstOrDefault().OrchardCoreId);
                            ErrorMessage += $"Multiple mappings for SOCCode-'{socCode.DisplayText}'-'{socCode.ContentItemId}' and SfId-'{sitefinityId}': <br /> ";
                            foreach (var mapping in mappings)
                            {
                                ErrorMessage += $"~ OrchardCoreId-'{mapping.OrchardCoreId}' and ContentType-'{mapping.ContentType}' <br />";
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage += $"Could not pull any mappings for SOCCode-'{socCode.DisplayText}'-'{socCode.ContentItemId}' and SfId-'{sitefinityId}' <br />";
                    }
                }

                socCode.SOCCode.ApprenticeshipStandards.ContentItemIds = orchardCoreIds?.ToArray();

                //MappingToolRepository.InsertMigrationMapping(socCode.SitefinityId, socCode.ContentItemId, ItemTypes.JobProfileSoc);
            }

            var jsonData = JsonConvert.SerializeObject(socCodes);
            var jsonSocCodes = JsonConvert.DeserializeObject<List<OcSocCode>>(jsonData);

            for (int i = 0; i < jsonSocCodes.Count(); i++)
            {
                jsonSocCodes[i].SitefinityId = socCodes[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(socCodes[i].SitefinityId, jsonSocCodes[i].ContentItemId, ItemTypes.JobProfileSoc);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-02-" + ItemTypes.JobProfileSoc + "-" + socCodes.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jsonSocCodes;
        }

        #endregion Private Methods -  - DynamicContentTypes - SOC Codes and Skills

        #region Private Methods - DynamicContentTypes - JobProfiles

        private List<OcJobProfile> GetJobProfiles()
        {
            var jobProfiles = new List<OcJobProfile>();
            var jobProfileUrls = jobProfileRepository.GetAllJobProfileUrls().OrderBy(jp => jp.Title).ToList();
            int jobProfileUrlsCount = 3; // jobProfileUrls.Count();

            for (int i = 0; i < jobProfileUrlsCount; i++)
            {
                var jobProfile = jobProfileRepository.GetJobProfileByUrlName(jobProfileUrls[i].UrlName);

                var orchardCoreHiddenAlternativeTitleIds = new List<string>();
                foreach (var sitefinityId in jobProfile.JobProfile.HiddenAlternativeTitleSf ?? new List<Guid>())
                {
                    var mappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId).ToList();
                    if (mappings != null || mappings.Count() == 0)
                    {
                        if (mappings.Count() == 1)
                        {
                            orchardCoreHiddenAlternativeTitleIds.Add(mappings[0].OrchardCoreId);
                        }
                        else
                        {
                            orchardCoreHiddenAlternativeTitleIds.Add(mappings.FirstOrDefault().OrchardCoreId);
                            ErrorMessage += $"Multiple mappings for JP -'{jobProfile.DisplayText}'-'{jobProfile.ContentItemId}' and SfId-'{sitefinityId}' - ContentType - 'HiddenAlternativeTitle': <br /> ";
                            foreach (var mapping in mappings)
                            {
                                ErrorMessage += $"~ OrchardCoreId-'{mapping.OrchardCoreId}' and ContentType-'{mapping.ContentType}' <br />";
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage += $"Could not pull any mappings for JP -'{jobProfile.DisplayText}'-'{jobProfile.ContentItemId}' and SfId-'{sitefinityId}' <br />";
                    }
                }

                jobProfile.JobProfile.HiddenAlternativeTitle.ContentItemIds = orchardCoreHiddenAlternativeTitleIds?.ToArray();

                var jsonData = JsonConvert.SerializeObject(jobProfile);

                //var jsonSocCodes = JsonConvert.DeserializeObject<List<OcJobProfile>>(jsonData);
                //MappingToolRepository.InsertMigrationMapping(jobProfile.SitefinityId, jobProfile.ContentItemId, ItemTypes.JobProfile);
                var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-24-" + ItemTypes.JobProfile + $"-{i + 1}-" + jobProfile.DisplayText.Replace(" ", string.Empty) + ".json";
                System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

                jobProfiles.Add(jobProfile);
            }

            // Replace RelatedCareers
            // And then print the recipes for each Jobprofile
            return jobProfiles;
        }

        #endregion Private Methods -  - DynamicContentTypes - JobProfiles
    }
}