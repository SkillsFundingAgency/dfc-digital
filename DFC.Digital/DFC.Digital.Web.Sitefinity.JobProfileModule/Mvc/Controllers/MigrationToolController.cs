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
                case ItemTypes.Registration:
                    model.Registrations = GetRegistrations();
                    count = model.Registrations.Count;
                    break;
                case ItemTypes.Restriction:
                    model.Restrictions = GetRestrictions();
                    count = model.Restrictions.Count;
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
                case ItemTypes.ApprenticeshipStandards:
                    model.ApprenticeshipStandards = GetApprenticeshipStandards().ToList();
                    count = model.ApprenticeshipStandards.Count;
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
                case ItemTypes.JobProfileSoc:
                    model.SocCodes = GetSocCodes().ToList();
                    model.ErrorMessage = ErrorMessage;
                    count = model.SocCodes.Count;
                    break;
                case ItemTypes.JobProfile:
                    model.JobProfiles = jobProfileRepository.GetAllJobProfiles().ToList();
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

            foreach (var registration in registrations)
            {
                MappingToolRepository.InsertMigrationMapping(registration.SitefinityId, registration.ContentItemId, ItemTypes.Registration);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-01-" + ItemTypes.Registration + "-" + registrations.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return registrations;
        }

        private List<OcRestriction> GetRestrictions()
        {
            var restrictions = dynamicModuleRepository.GetAllRestrictions().ToList();

            var jsonData = JsonConvert.SerializeObject(restrictions);

            foreach (var restriction in restrictions)
            {
                MappingToolRepository.InsertMigrationMapping(restriction.SitefinityId, restriction.ContentItemId, ItemTypes.Restriction);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-02-" + ItemTypes.Restriction + "-" + restrictions.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return restrictions;
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

        private List<OcCollegeLink> GetCollegeLinks()
        {
            var collegeLinks = dynamicModuleRepository.GetAllCollegeLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(collegeLinks);

            foreach (var collegeLink in collegeLinks)
            {
                MappingToolRepository.InsertMigrationMapping(collegeLink.SitefinityId, collegeLink.ContentItemId, ItemTypes.CollegeLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-04-" + ItemTypes.CollegeLink + "-" + collegeLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return collegeLinks;
        }

        private List<OcUniversityLink> GetUniversityLinks()
        {
            var universityLinks = dynamicModuleRepository.GetAllUniversityLinks().ToList();

            var jsonData = JsonConvert.SerializeObject(universityLinks);

            foreach (var universityLink in universityLinks)
            {
                MappingToolRepository.InsertMigrationMapping(universityLink.SitefinityId, universityLink.ContentItemId, ItemTypes.UniversityLink);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-05-" + ItemTypes.UniversityLink + "-" + universityLinks.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return universityLinks;
        }

        private List<OcApprenticeshipRequirement> GetApprenticeshipRequirements()
        {
            var apprenticeshipRequirements = dynamicModuleRepository.GetAllApprenticeshipRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(apprenticeshipRequirements);

            foreach (var apprenticeshipRequirement in apprenticeshipRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipRequirement.SitefinityId, apprenticeshipRequirement.ContentItemId, ItemTypes.ApprenticeshipRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-06-" + ItemTypes.ApprenticeshipRequirement + "-" + apprenticeshipRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipRequirements;
        }

        private List<OcCollegeRequirement> GetCollegeRequirements()
        {
            var collegeRequirements = dynamicModuleRepository.GetAllCollegeRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(collegeRequirements);

            foreach (var collegeRequirement in collegeRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(collegeRequirement.SitefinityId, collegeRequirement.ContentItemId, ItemTypes.CollegeRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-07-" + ItemTypes.CollegeRequirement + "-" + collegeRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return collegeRequirements;
        }

        private List<OcUniversityRequirement> GetUniversityRequirements()
        {
            var universityRequirements = dynamicModuleRepository.GetAllUniversityRequirements().ToList();

            var jsonData = JsonConvert.SerializeObject(universityRequirements);

            foreach (var universityRequirement in universityRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(universityRequirement.SitefinityId, universityRequirement.ContentItemId, ItemTypes.UniversityRequirement);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-08-" + ItemTypes.UniversityRequirement + "-" + universityRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return universityRequirements;
        }

        private List<OcUniform> GetUniforms()
        {
            var uniforms = dynamicModuleRepository.GetAllUniforms().ToList();

            var jsonData = JsonConvert.SerializeObject(uniforms);

            foreach (var uniform in uniforms)
            {
                MappingToolRepository.InsertMigrationMapping(uniform.SitefinityId, uniform.ContentItemId, ItemTypes.Uniform);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-09-" + ItemTypes.Uniform + "-" + uniforms.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return uniforms;
        }

        private List<OcLocation> GetLocations()
        {
            var locations = dynamicModuleRepository.GetAllLocations().ToList();

            var jsonData = JsonConvert.SerializeObject(locations);

            foreach (var location in locations)
            {
                MappingToolRepository.InsertMigrationMapping(location.SitefinityId, location.ContentItemId, ItemTypes.Location);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-10-" + ItemTypes.Location + "-" + locations.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return locations;
        }

        private List<OcEnvironment> GetEnvironments()
        {
            var environments = dynamicModuleRepository.GetAllEnvironments().ToList();

            var jsonData = JsonConvert.SerializeObject(environments);

            foreach (var environment in environments)
            {
                MappingToolRepository.InsertMigrationMapping(environment.SitefinityId, environment.ContentItemId, ItemTypes.Environment);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-11-" + ItemTypes.Environment + "-" + environments.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return environments;
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

            foreach (var apprenticeshipEntryRequirement in hiddenAlternativeTitles)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipEntryRequirement.SitefinityId, apprenticeshipEntryRequirement.ContentItemId, ItemTypes.HiddenAlternativeTitle);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-12-" + ItemTypes.HiddenAlternativeTitle + "-" + hiddenAlternativeTitles.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return hiddenAlternativeTitles;
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

            foreach (var apprenticeshipEntryRequirement in apprenticeshipEntryRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipEntryRequirement.SitefinityId, apprenticeshipEntryRequirement.ContentItemId, ItemTypes.ApprenticeshipEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-13-" + ItemTypes.ApprenticeshipEntryRequirements + "-" + apprenticeshipEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipEntryRequirements;
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

            foreach (var workingPattern in workingPatterns)
            {
                MappingToolRepository.InsertMigrationMapping(workingPattern.SitefinityId, workingPattern.ContentItemId, ItemTypes.WorkingPattern);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-14-" + ItemTypes.WorkingPattern + "-" + workingPatterns.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return workingPatterns;
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
                PageLocationPart = new Pagelocationpart() { UrlName = ht.UrlName, DefaultPageForLocation = false, RedirectLocations = null, FullUrl = $"/{ht.UrlName}" },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/jobprofilecategory/{ht.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(jobProfileCategories);

            foreach (var jobProfileCategory in jobProfileCategories)
            {
                MappingToolRepository.InsertMigrationMapping(jobProfileCategory.SitefinityId, jobProfileCategory.ContentItemId, ItemTypes.JobProfileCategories);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-15-" + ItemTypes.JobProfileCategories + "-" + jobProfileCategories.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jobProfileCategories;
        }

        private IEnumerable<OcApprenticeshipStandard> GetApprenticeshipStandards()
        {
            var apprenticeshipStandards = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.ApprenticeshipStandards).Select(ft => new OcApprenticeshipStandard
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentItemVersionId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = OcItemTypes.ApprenticeshipStandard,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,
                PublishedUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow,
                Owner = OrchardCoreFields.Owner,
                Author = OrchardCoreFields.Author,
                UniqueTitlePart = new Uniquetitlepart() { Title = ft.Title },
                TitlePart = new Titlepart() { Title = ft.Title },
                ApprenticeshipStandard = new Apprenticeshipstandard() { Description = new OcDescriptionText { Text = ft.Description }, LARScode = new Larscode { Text = ft.UrlName } },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/apprenticeshipstandard/{ft.Id}" },
                AuditTrailPart = new Audittrailpart() { Comment = null, ShowComment = false }
            });

            var jsonData = JsonConvert.SerializeObject(apprenticeshipStandards);

            foreach (var apprenticeshipStandard in apprenticeshipStandards)
            {
                MappingToolRepository.InsertMigrationMapping(apprenticeshipStandard.SitefinityId, apprenticeshipStandard.ContentItemId, ItemTypes.ApprenticeshipStandards);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-16-" + ItemTypes.ApprenticeshipStandards + "-" + apprenticeshipStandards.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return apprenticeshipStandards;
        }

        private IEnumerable<OcUniversityEntryRequirement> GetUniversityEntryRequirements()
        {
            var universityEntryRequirements = flatTaxonomyRepository.GetMany(ft => ft.Taxonomy.Name == ItemTypes.UniversityEntryRequirements).Select(ft => new OcUniversityEntryRequirement
            {
                SitefinityId = ft.Id,
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),

                //ContentItemVersionId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.UniversityEntryRequirements,
                DisplayText = ft.Title,
                Latest = true,
                Published = true,
                ModifiedUtc = ft.LastModified,

                //PublishedUtc = DateTime.UtcNow,
                //CreatedUtc = DateTime.UtcNow,
                //Owner = OrchardCoreFields.Owner,
                //Author = OrchardCoreFields.Author,
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
                jsonUniversityEntryRequirements.ElementAt(i).SitefinityId = universityEntryRequirements.ElementAt(i).SitefinityId;
                MappingToolRepository.InsertMigrationMapping(universityEntryRequirements.ElementAt(i).SitefinityId, jsonUniversityEntryRequirements.ElementAt(i).ContentItemId, ItemTypes.UniversityEntryRequirements);
            }

            //foreach (var universityEntryRequirement in jsonUniversityEntryRequirements)
            //{
            //    MappingToolRepository.InsertMigrationMapping(universityEntryRequirement.SitefinityId, universityEntryRequirement.ContentItemId, ItemTypes.UniversityEntryRequirements);
            //}
            //for (int i = 0; i < universityEntryRequirements.Count(); i++)
            //{
            //    var contentItemId = GetIt(); //OrchardCoreIdGenerator.GenerateUniqueId();
            //    universityEntryRequirements.ElementAt(i).ContentItemId = contentItemId;
            //    MappingToolRepository.InsertMigrationMapping(universityEntryRequirements.ElementAt(i).SitefinityId, contentItemId, ItemTypes.UniversityEntryRequirements);
            //}

            //    foreach (var universityEntryRequirement in universityEntryRequirements)
            //{
            //    var contentItemId = OrchardCoreIdGenerator.GenerateUniqueId();
            //    universityEntryRequirement.ContentItemId = contentItemId;
            //    MappingToolRepository.InsertMigrationMapping(universityEntryRequirement.SitefinityId, contentItemId, ItemTypes.UniversityEntryRequirements);
            //}
            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-17-" + ItemTypes.UniversityEntryRequirements + "-" + universityEntryRequirements.Count().ToString() + ".json";
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

            foreach (var collegeEntryRequirement in collegeEntryRequirements)
            {
                MappingToolRepository.InsertMigrationMapping(collegeEntryRequirement.SitefinityId, collegeEntryRequirement.ContentItemId, ItemTypes.CollegeEntryRequirements);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-18-" + ItemTypes.CollegeEntryRequirements + "-" + collegeEntryRequirements.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return collegeEntryRequirements;
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

            foreach (var jobProfileSpecialism in jobProfileSpecialisms)
            {
                MappingToolRepository.InsertMigrationMapping(jobProfileSpecialism.SitefinityId, jobProfileSpecialism.ContentItemId, ItemTypes.JobProfileSpecialism);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-19-" + ItemTypes.JobProfileSpecialism + "-" + jobProfileSpecialisms.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return jobProfileSpecialisms;
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

            foreach (var workingPatternDetail in workingPatternDetails)
            {
                MappingToolRepository.InsertMigrationMapping(workingPatternDetail.SitefinityId, workingPatternDetail.ContentItemId, ItemTypes.WorkingPatternDetails);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-20-" + ItemTypes.WorkingPatternDetails + "-" + workingPatternDetails.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return workingPatternDetails;
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

            foreach (var workingHoursDetail in workingHoursDetails)
            {
                MappingToolRepository.InsertMigrationMapping(workingHoursDetail.SitefinityId, workingHoursDetail.ContentItemId, ItemTypes.WorkingHoursDetails);
            }

            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-21-" + ItemTypes.WorkingHoursDetails + "-" + workingHoursDetails.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return workingHoursDetails;
        }

        #endregion Private Methods - Classifications

        #region Private Methods - DynamicContentTypes - SOC Codes and Skills

        private List<OcSocCode> GetSocCodes()
        {
            var socCodes = dynamicModuleRepository.GetAllSOCCodes().ToList();

            foreach (var socCode in socCodes)
            {
                var orchardCoreIds = new List<string>();
                foreach (var sitefinityId in socCode.SOCCode.ApprenticeshipStandardsSitefinityIds.SitefinityIds)
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

                MappingToolRepository.InsertMigrationMapping(socCode.SitefinityId, socCode.ContentItemId, ItemTypes.JobProfileSoc);
            }

            var jsonData = JsonConvert.SerializeObject(socCodes);
            var fullPathAndFileName = JsonFilePath + DateTime.Now.ToString("yyMMddHHmm") + "-22-" + ItemTypes.JobProfileSoc + "-" + socCodes.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            return socCodes;
        }

        #endregion Private Methods -  - DynamicContentTypes - SOC Codes and Skills
    }
}