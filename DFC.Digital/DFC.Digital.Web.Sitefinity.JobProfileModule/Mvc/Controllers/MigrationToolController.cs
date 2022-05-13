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
using OfficeOpenXml;
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

        [DisplayName("Json FilePath")]
        public string JsonFilePath { get; set; } = @"D:\ESFA-GitHub\dfc-digital\DFC.Digital\DFC.Digital.Web.Sitefinity\OrchardCoreRecipes\";

        [DisplayName("Recipe Beginning")]
        public string RecipeBeginning { get; set; } = "{ \"name\": \"\", \"displayName\": \"\", \"description\": \"\", \"author\": \"\", \"website\": \"\", \"version\": \"\", \"issetuprecipe\": false, \"categories\": [], \"tags\": [],\"steps\": [{ \"name\": \"content\", \"data\": ";

        [DisplayName("Recipe End")]
        public string RecipeEnd { get; set; } = "}]}";

        [DisplayName("Recipe Beginning Single")]
        public string RecipeBeginningSingle { get; set; } = "{ \"name\": \"\", \"displayName\": \"\", \"description\": \"\", \"author\": \"\", \"website\": \"\", \"version\": \"\", \"issetuprecipe\": false, \"categories\": [], \"tags\": [],\"steps\": [{ \"name\": \"content\", \"data\": [";

        [DisplayName("Recipe End Single")]
        public string RecipeEndSingle { get; set; } = "]}]}";

        [DisplayName("Recipe Beginning CSharpContent")]
        public string RecipeBeginningCSharpContent { get; set; } = "{ \"name\": \"PersonalityFilteringQuestion_Draft\", \"displayName\": \"PersonalityFilteringQuestion_Draft\", \"description\": \"\", \"author\": \"\", \"website\": \"\", \"version\": \"\", \"issetuprecipe\": false, \"categories\": [], \"tags\": [], \"steps\": [ { \"name\": \"CSharpContent\", \"data\": ";

        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }

        [DisplayName("Is it FirstRun")]
        public bool IsRirstRun { get; set; } = true;

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
                    model.ErrorMessage = ErrorMessage;
                    count = model.JobProfiles.Count;
                    break;
                case "JobProfileRCP":
                    model.JobProfiles = GetJobProfilesRCP();
                    model.ErrorMessage = ErrorMessage;
                    count = model.JobProfiles.Count;
                    break;
                case "JobProfileSkills":
                    model.JobProfiles = GetJobProfilesSkills();
                    model.ErrorMessage = ErrorMessage;
                    count = model.JobProfiles.Count;
                    break;
                case "FilteringQuestions":
                    model.FilteringQuestions = GetFilteringQuestions();
                    model.ErrorMessage = ErrorMessage;
                    count = model.FilteringQuestions.Count;
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

            var fullPathAndFileName = JsonFilePath + "12-" + ItemTypes.Registration + "-" + registrations.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "13-" + ItemTypes.Restriction + "-" + restrictions.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "03-" + ItemTypes.ApprenticeshipLink + "-" + apprenticeshipLinks.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "04-" + ItemTypes.CollegeLink + "-" + collegeLinks.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "05-" + ItemTypes.UniversityLink + "-" + universityLinks.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "06-" + ItemTypes.ApprenticeshipRequirement + "-" + apprenticeshipRequirements.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "07-" + ItemTypes.CollegeRequirement + "-" + collegeRequirements.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "08-" + ItemTypes.UniversityRequirement + "-" + universityRequirements.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "09-" + ItemTypes.Uniform + "-" + uniforms.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "10-" + ItemTypes.Location + "-" + locations.Count().ToString() + ".json";
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

            var fullPathAndFileName = JsonFilePath + "11-" + ItemTypes.Environment + "-" + environments.Count().ToString() + ".json";
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

            var fileName = "14-" + ItemTypes.HiddenAlternativeTitle + "-" + hiddenAlternativeTitles.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            var data = new List<object[]>();
            foreach (var hiddenAlternativeTitle in jsonHiddenAlternativeTitles)
            {
                var dataItem = new object[] { hiddenAlternativeTitle.SitefinityId, hiddenAlternativeTitle.DisplayText, hiddenAlternativeTitle.HiddenAlternativeTitle?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.HiddenAlternativeTitle, fileName);
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

            var fileName = "15-" + ItemTypes.ApprenticeshipEntryRequirements + "-" + apprenticeshipEntryRequirements.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            var data = new List<object[]>();
            foreach (var apprenticeshipEntryRequirement in jsonApprenticeshipEntryRequirements)
            {
                var dataItem = new object[] { apprenticeshipEntryRequirement.SitefinityId, apprenticeshipEntryRequirement.DisplayText, apprenticeshipEntryRequirement.ApprenticeshipEntryRequirements?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.ApprenticeshipEntryRequirements, fileName);
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

            var fileName = "16-" + ItemTypes.WorkingPattern + "-" + workingPatterns.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);

            var data = new List<object[]>();
            foreach (var workingPattern in jsonWorkingPatterns)
            {
                var dataItem = new object[] { workingPattern.SitefinityId, workingPattern.DisplayText, workingPattern.WorkingPatterns?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.WorkingPattern, fileName);

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

            var fileName = "17-" + SitefinityFields.RelatedJobProfileCategories + "-" + jobProfileCategories.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var jobProfileCategory in jsonJobProfileCategories)
            {
                var dataItem = new object[] { jobProfileCategory.SitefinityId, jobProfileCategory.DisplayText, jobProfileCategory.JobProfileCategory?.Description?.Text, jobProfileCategory.PageLocationPart.UrlName, jobProfileCategory.PageLocationPart.FullUrl };
                data.Add(dataItem);
            }

            WriteExelFile(data, SitefinityFields.RelatedJobProfileCategories, fileName);

            return jsonJobProfileCategories;
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
            int batchSize = 400;
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

                var currentFullPathAndFileName = JsonFilePath + "01-" + SitefinityFields.ApprenticeshipStandards + $"-{idx + 1}-" + ((idx * batchSize) + jsonCurruntApprenticeshipStandards.Count()) + ".json";

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

            var fileName = "01-" + SitefinityFields.ApprenticeshipStandards + "-" + apprenticeshipStandards.Count().ToString();
            var data = new List<object[]>();
            foreach (var apprenticeshipStandard in jsonApprenticeshipStandards)
            {
                var dataItem = new object[] { apprenticeshipStandard.SitefinityId, apprenticeshipStandard.DisplayText, apprenticeshipStandard.ApprenticeshipStandard?.Description?.Text, apprenticeshipStandard.ApprenticeshipStandard?.LARScode?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, SitefinityFields.ApprenticeshipStandards, fileName);

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

            var fileName = "18-" + ItemTypes.UniversityEntryRequirements + "-" + universityEntryRequirements.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var universityEntryRequirement in jsonUniversityEntryRequirements)
            {
                var dataItem = new object[] { universityEntryRequirement.SitefinityId, universityEntryRequirement.DisplayText, universityEntryRequirement.UniversityEntryRequirements?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.UniversityEntryRequirements, fileName);

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

            var fileName = "19-" + ItemTypes.CollegeEntryRequirements + "-" + collegeEntryRequirements.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var collegeEntryRequirement in jsonCollegeEntryRequirements)
            {
                var dataItem = new object[] { collegeEntryRequirement.SitefinityId, collegeEntryRequirement.DisplayText, collegeEntryRequirement.CollegeEntryRequirements?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.CollegeEntryRequirements, fileName);

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

            var fileName = "20-" + ItemTypes.JobProfileSpecialism + "-" + jobProfileSpecialisms.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var jobProfileSpecialism in jsonJobProfileSpecialism)
            {
                var dataItem = new object[] { jobProfileSpecialism.SitefinityId, jobProfileSpecialism.DisplayText, jobProfileSpecialism.JobProfileSpecialism?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.JobProfileSpecialism, fileName);

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

            var fileName = "21-" + ItemTypes.WorkingPatternDetails + "-" + workingPatternDetails.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var workingPatternDetail in jsonWorkingPatternDetails)
            {
                var dataItem = new object[] { workingPatternDetail.SitefinityId, workingPatternDetail.DisplayText, workingPatternDetail.WorkingPatternDetail?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.WorkingPatternDetails, fileName);

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

            var fileName = "22-" + ItemTypes.WorkingHoursDetails + "-" + workingHoursDetails.Count().ToString();
            var fullPathAndFileName = JsonFilePath + fileName + ".json";

            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            var data = new List<object[]>();
            foreach (var workingHoursDetail in jsonWorkingHoursDetails)
            {
                var dataItem = new object[] { workingHoursDetail.SitefinityId, workingHoursDetail.DisplayText, workingHoursDetail.WorkingHoursDetail?.Description?.Text };
                data.Add(dataItem);
            }

            WriteExelFile(data, ItemTypes.WorkingHoursDetails, fileName);

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

                MappingToolRepository.InsertMigrationMapping(socCode.SitefinityId, socCode.ContentItemId, ItemTypes.JobProfileSoc);
            }

            var jsonData = JsonConvert.SerializeObject(socCodes);
            var jsonSocCodes = JsonConvert.DeserializeObject<List<OcSocCode>>(jsonData);

            int batchSize = 10;
            int numberOfFiles = jsonSocCodes.Count() / batchSize;

            for (int idx = 0; idx <= numberOfFiles; idx++)
            {
                var currentSocCodes = jsonSocCodes.Skip(idx * batchSize).Take(batchSize).ToList();

                var currentJsonData = JsonConvert.SerializeObject(currentSocCodes);

                var currentFullPathAndFileName = JsonFilePath + @"SocCodesSplits\" + $"02-SOCcodes-{idx + 1}-" + ((idx * batchSize) + currentSocCodes.Count()) + ".json";
                System.IO.File.WriteAllText(currentFullPathAndFileName, RecipeBeginning + currentJsonData + RecipeEnd);
            }

            /*
            for (int i = 0; i < jsonSocCodes.Count(); i++)
            {
                jsonSocCodes[i].SitefinityId = socCodes[i].SitefinityId;
                MappingToolRepository.InsertMigrationMapping(socCodes[i].SitefinityId, jsonSocCodes[i].ContentItemId, ItemTypes.JobProfileSoc);
            }

            var fullPathAndFileName = JsonFilePath + "02-" + ItemTypes.JobProfileSoc + "-" + socCodes.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginning + jsonData + RecipeEnd);
            */

            return socCodes;
        }

        #endregion Private Methods -  - DynamicContentTypes - SOC Codes and Skills

        #region Private Methods - DynamicContentTypes - JobProfiles

        private List<OcJobProfile> GetJobProfiles()
        {
            IsRirstRun = false; // Overides SF setting, if uncommented.
            var jobProfiles = new List<OcJobProfile>();
            var jobProfileUrls = dynamicModuleRepository.GetAllJobProfileUrls().OrderBy(jp => jp.Title).ToList();
            int jobProfileUrlsCount = jobProfileUrls.Count();

            for (int i = 0; i < jobProfileUrlsCount; i++)
            {
                var jobProfile = dynamicModuleRepository.GetJobProfileByUrlName(jobProfileUrls[i].UrlName);

                // HiddenAlternativeTitle
                jobProfile.JobProfile.HiddenAlternativeTitle.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.HiddenAlternativeTitleSf, ItemTypes.HiddenAlternativeTitle);

                // WorkingHoursDetails
                jobProfile.JobProfile.WorkingHoursDetails.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.WorkingHoursDetailsSf, OcItemTypes.WorkingHoursDetail);

                // WorkingPattern
                jobProfile.JobProfile.WorkingPattern.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.WorkingPatternSf, ItemTypes.WorkingPattern);

                // WorkingPatternDetails
                jobProfile.JobProfile.WorkingPatternDetails.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.WorkingPatternDetailsSf, ItemTypes.WorkingPatternDetails);

                // JobProfileSpecialism
                jobProfile.JobProfile.JobProfileSpecialism.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.JobProfileSpecialismSf, ItemTypes.JobProfileSpecialism);

                // UniversityEntryRequirements
                jobProfile.JobProfile.UniversityEntryRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.UniversityEntryRequirementsSf, ItemTypes.UniversityEntryRequirements);

                // UniversityRequirements
                jobProfile.JobProfile.RelatedUniversityRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedUniversityRequirementsSf, ItemTypes.UniversityRequirement);

                // UniversityLinks
                jobProfile.JobProfile.RelatedUniversityLinks.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedUniversityLinksSf, ItemTypes.UniversityLink);

                // CollegeEntryRequirements
                jobProfile.JobProfile.CollegeEntryRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.CollegeEntryRequirementsSf, ItemTypes.CollegeEntryRequirements);

                // CollegeRequirements
                jobProfile.JobProfile.RelatedCollegeRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedCollegeRequirementsSf, ItemTypes.CollegeRequirement);

                // CollegeLink
                jobProfile.JobProfile.RelatedCollegeLinks.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedCollegeLinksSf, ItemTypes.CollegeLink);

                // ApprenticeshipEntryRequirements
                jobProfile.JobProfile.ApprenticeshipEntryRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.ApprenticeshipEntryRequirementsSf, ItemTypes.ApprenticeshipEntryRequirements);

                // ApprenticeshipRequirements
                jobProfile.JobProfile.RelatedApprenticeshipRequirements.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedApprenticeshipRequirementsSf, ItemTypes.ApprenticeshipRequirement);

                // ApprenticeshipLinks
                jobProfile.JobProfile.RelatedApprenticeshipLinks.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedApprenticeshipLinksSf, ItemTypes.ApprenticeshipLink);

                // Restrictions
                jobProfile.JobProfile.Relatedrestrictions.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedrestrictionsSf, ItemTypes.Restriction);

                // JobProfileCategories
                jobProfile.JobProfile.JobProfileCategory.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.JobProfileCategorySf, ItemTypes.JobProfileCategories);

                // Locations
                jobProfile.JobProfile.RelatedLocations.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedLocationsSf, ItemTypes.Location);

                // Environments
                jobProfile.JobProfile.RelatedEnvironments.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedEnvironmentsSf, ItemTypes.Environment);

                // Uniforms
                jobProfile.JobProfile.RelatedUniforms.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedUniformsSf, ItemTypes.Uniform);

                // SOCCodes
                jobProfile.JobProfile.SOCCode.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.SOCCodeSf, ItemTypes.JobProfileSoc);

                // Registrations
                jobProfile.JobProfile.RelatedRegistrations.ContentItemIds = GetOrchardCoreIds(jobProfile.DisplayText, jobProfile.ContentItemId, jobProfile.JobProfile.RelatedRegistrationsSf, ItemTypes.Registration);

                if (IsRirstRun)
                {
                    // Preserve SitefinityId & OrchardCoreId per JobProfile by storing them in the database
                    MappingToolRepository.InsertMigrationMapping(jobProfile.SitefinityId, jobProfile.ContentItemId, ItemTypes.JobProfile, jobProfile.ContentItemVersionId);
                }

                jobProfiles.Add(jobProfile);
            }

            for (int i = 0; i < jobProfileUrlsCount; i++)
            {
                if (!IsRirstRun)
                {
                    // Replace RelatedCareerProfiles SitefinityIds with OrchardCoreIds
                    jobProfiles[i].JobProfile.Relatedcareerprofiles.ContentItemIds = GetOrchardCoreIds(jobProfiles[i].DisplayText, jobProfiles[i].ContentItemId, jobProfiles[i].JobProfile.RelatedcareerprofilesSf, ItemTypes.JobProfile);

                    // Restore original ContentItemId from the first run
                    jobProfiles[i].ContentItemId = GetJobProfileContentItemId(jobProfiles[i].DisplayText, jobProfiles[i].ContentItemId, jobProfiles[i].SitefinityId, ItemTypes.JobProfile);

                    // Restore original ContentItemVersionId from the first run
                    jobProfiles[i].ContentItemVersionId = GetJobProfileContentItemVersionId(jobProfiles[i].DisplayText, jobProfiles[i].ContentItemId, jobProfiles[i].SitefinityId, ItemTypes.JobProfile);
                }

                // And then print the recipes for each Jobprofile
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };
                var jsonData = JsonConvert.SerializeObject(jobProfiles[i], jsonSerializerSettings);
                var fullPathAndFileName = JsonFilePath + "24-" + ItemTypes.JobProfile + $"-{i + 1}-" + jobProfiles[i].DisplayText.Replace(" ", string.Empty) + ".json";
                System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginningSingle + jsonData + RecipeEndSingle);
            }

            // Split all JobProfiles into batches of
            int batchSize = 15;
            int numberOfFiles = jobProfileUrlsCount / batchSize;

            for (int idx = 0; idx <= numberOfFiles; idx++)
            {
                var currentJobProfiles = jobProfiles.Skip(idx * batchSize).Take(batchSize).ToList();

                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };
                var currentJsonData = JsonConvert.SerializeObject(currentJobProfiles, jsonSerializerSettings);

                var currentFullPathAndFileName = JsonFilePath + @"JobProfilesSplits\" + "24-" + ItemTypes.JobProfile + $"-{idx + 1}-" + ((idx * batchSize) + currentJobProfiles.Count()) + ".json";
                System.IO.File.WriteAllText(currentFullPathAndFileName, RecipeBeginning + currentJsonData + RecipeEnd);
            }

            return jobProfiles;
        }

        private List<OcJobProfile> GetJobProfilesRCP()
        {
            var jobProfiles = new List<OcJobProfile>();
            var jobProfileUrls = dynamicModuleRepository.GetAllJobProfileUrls().OrderBy(jp => jp.Title).ToList();
            int jobProfileUrlsCount = jobProfileUrls.Count();

            var data = new List<object[]>();

            for (int i = 0; i < jobProfileUrlsCount; i++)
            {
                var jobProfile = dynamicModuleRepository.GetJobProfileByUrlNameRCP(jobProfileUrls[i].UrlName);

                var dataItem = new List<string> { jobProfile.SitefinityId.ToString(), jobProfile.DisplayText };

                var rcpCount = jobProfile.JobProfile.RelatedcareerprofilesSfTitles.Count();
                var addRcpCount = 10 - rcpCount;
                dataItem.AddRange(jobProfile.JobProfile.RelatedcareerprofilesSfTitles);
                dataItem.AddRange(AddEmptyRCPs(addRcpCount));

                data.Add(dataItem.ToArray<object>());

                jobProfiles.Add(jobProfile);
            }

            WriteExelFileRCP(data);

            return jobProfiles;
        }

        private List<OcJobProfile> GetJobProfilesSkills()
        {
            var jobProfiles = new List<OcJobProfile>();
            var jobProfileUrls = dynamicModuleRepository.GetAllJobProfileUrls().OrderBy(jp => jp.Title).ToList();
            int jobProfileUrlsCount = jobProfileUrls.Count();

            var data = new List<object[]>();

            for (int i = 0; i < jobProfileUrlsCount; i++)
            {
                var jobProfile = dynamicModuleRepository.GetJobProfileByUrlNameSkills(jobProfileUrls[i].UrlName);

                var dataItem = new List<string> { jobProfile.SitefinityId.ToString(), jobProfile.DisplayText, jobProfile.JobProfile.SOCCodeSfTitles.FirstOrDefault() };

                var skillsCount = jobProfile.JobProfile.RelatedSkillsSfTitles.Count();
                var addSkillsCount = 21 - skillsCount;
                dataItem.AddRange(jobProfile.JobProfile.RelatedSkillsSfTitles);
                dataItem.AddRange(AddEmptyRCPs(addSkillsCount));

                data.Add(dataItem.ToArray<object>());

                jobProfiles.Add(jobProfile);
            }

            WriteExelFileSkills(data);

            return jobProfiles;
        }

        private List<string> AddEmptyRCPs(int count)
        {
            var addEmptyRCPs = new List<string>();

            for (var i = 0; i < count; i++)
            {
                addEmptyRCPs.Add(string.Empty);
            }

            return addEmptyRCPs;
        }

        private string[] GetOrchardCoreIds(string jobProfileDisplayText, string jobProfileContentItemId, List<Guid> sitefinityIds, string contentType)
        {
            var orchardCoreIds = new List<string>();
            foreach (var sitefinityId in sitefinityIds ?? new List<Guid>())
            {
                var mappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId).ToList();
                if (mappings != null && mappings?.Count() != 0)
                {
                    if (mappings.Count() == 1)
                    {
                        orchardCoreIds.Add(mappings[0].OrchardCoreId);
                    }
                    else
                    {
                        orchardCoreIds.Add(mappings.FirstOrDefault().OrchardCoreId);
                        ErrorMessage += $"Multiple mappings for JP -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId- '{sitefinityId}', JPContentType- '{contentType}': <br /> ";
                        foreach (var mapping in mappings)
                        {
                            ErrorMessage += $"~ OrchardCoreId-'{mapping.OrchardCoreId}' and MappingContentType-'{mapping.ContentType}' <br />";
                        }
                    }
                }
                else
                {
                    ErrorMessage += $"Could not pull any mappings for JP -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId-'{sitefinityId}',  JPContentType- '{contentType}' <br />";
                }
            }

            return orchardCoreIds.ToArray();
        }

        private string GetJobProfileContentItemId(string jobProfileDisplayText, string jobProfileContentItemId, Guid sitefinityId, string contentType)
        {
            var contentItemId = string.Empty;

            var mappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId).ToList();
            if (mappings != null && mappings?.Count() != 0)
            {
                if (mappings.Count() == 1)
                {
                    contentItemId = mappings[0].OrchardCoreId;
                }
                else
                {
                    contentItemId = mappings.FirstOrDefault().OrchardCoreId;
                    ErrorMessage += $"Multiple mappings for JP ContentItemId -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId- '{sitefinityId}', JPContentType- '{contentType}': <br /> ";
                    foreach (var mapping in mappings)
                    {
                        ErrorMessage += $"~ OrchardCoreId-'{mapping.OrchardCoreId}' and MappingContentType-'{mapping.ContentType}' <br />";
                    }
                }
            }
            else
            {
                ErrorMessage += $"Could not pull any mappings for JP ContentItemId -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId-'{sitefinityId}',  JPContentType- '{contentType}' <br />";
            }

            return contentItemId;
        }

        private string GetJobProfileContentItemVersionId(string jobProfileDisplayText, string jobProfileContentItemId, Guid sitefinityId, string contentType)
        {
            var contentItemVersionId = string.Empty;

            var mappings = MappingToolRepository.GetMigrationMappingBySitefinityId(sitefinityId).ToList();
            if (mappings != null && mappings?.Count() != 0)
            {
                if (mappings.Count() == 1)
                {
                    contentItemVersionId = mappings[0].ContentItemVersionId;
                }
                else
                {
                    contentItemVersionId = mappings.FirstOrDefault().ContentItemVersionId;
                    ErrorMessage += $"Multiple mappings (ContentItemVersionId) for JP ContentItemId -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId- '{sitefinityId}', JPContentType- '{contentType}': <br /> ";
                    foreach (var mapping in mappings)
                    {
                        ErrorMessage += $"~ OrchardCoreId-'{mapping.OrchardCoreId}' and MappingContentType-'{mapping.ContentType}' <br />";
                    }
                }
            }
            else
            {
                ErrorMessage += $"Could not pull any mappings (ContentItemVersionId) for JP ContentItemId -'{jobProfileDisplayText}'-'{jobProfileContentItemId}', SfId-'{sitefinityId}',  JPContentType- '{contentType}' <br />";
            }

            return contentItemVersionId;
        }

        #endregion Private Methods - DynamicContentTypes - JobProfiles

        #region Private Methods - WriteExelFiles

        private void WriteExelFile(List<object[]> data, string contentType, string fileName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage())
            {
                //Add Worksheets in Excel file
                excel.Workbook.Worksheets.Add(contentType);

                //Create Excel file in Uploads folder of your project
                FileInfo excelFile = new FileInfo(Server.MapPath($"~/OrchardCoreExcelFiles/{fileName}.xlsx"));

                //Add header row columns name in string list array
                var headerRow = new List<string[]>();

                if (contentType.Equals(SitefinityFields.RelatedJobProfileCategories))
                {
                    headerRow = new List<string[]>()
                    {
                       new string[] { "Id", "Title_en", "Description_en", "UrlName", "FullUrl" }
                    };
                }
                else if (contentType.Equals(SitefinityFields.ApprenticeshipStandards))
                {
                    headerRow = new List<string[]>()
                    {
                       new string[] { "Id", "Title_en", "Description_en", "LARScode" }
                    };
                }
                else
                {
                    headerRow = new List<string[]>()
                    {
                       new string[] { "Id", "Title_en", "Description_en" }
                    };
                }

                // Get the header range
                string range = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // get the workSheet in which you want to create header
                var worksheet = excel.Workbook.Worksheets[contentType];

                // Popular header row data
                worksheet.Cells[range].LoadFromArrays(headerRow);

                //show header cells with different style
                worksheet.Cells[range].Style.Font.Bold = true;
                worksheet.Cells[range].Style.Font.Size = 11;
                worksheet.Cells[range].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                //add the data in worksheet, here .Cells[2,1] 2 is rowNumber while 1 is column number
                worksheet.Cells[2, 1].LoadFromArrays(data);

                //Save Excel file
                excel.SaveAs(excelFile);
            }
        }

        private void WriteExelFileRCP(List<object[]> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage())
            {
                //Add Worksheets in Excel file
                excel.Workbook.Worksheets.Add("JobProfile RCPs");

                //Create Excel file in Uploads folder of your project
                FileInfo excelFile = new FileInfo(Server.MapPath($"~/OrchardCoreExcelFiles/JobProfileRCPs.xlsx"));

                //Add header row columns name in string list array
                var headerRow = new List<string[]>();

                headerRow = new List<string[]>()
                    {
                       new string[] { "Id", "Title", "RCP01", "RCP02", "RCP03", "RCP04", "RCP05", "RCP06", "RCP07", "RCP08", "RCP09", "RCP10" }
                    };

                // Get the header range
                string range = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // get the workSheet in which you want to create header
                var worksheet = excel.Workbook.Worksheets["JobProfile RCPs"];

                // Popular header row data
                worksheet.Cells[range].LoadFromArrays(headerRow);

                //show header cells with different style
                worksheet.Cells[range].Style.Font.Bold = true;
                worksheet.Cells[range].Style.Font.Size = 11;
                worksheet.Cells[range].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                //add the data in worksheet, here .Cells[2,1] 2 is rowNumber while 1 is column number
                //worksheet.Cells[2, 1].LoadFromCollection(data);
                worksheet.Cells[2, 1].LoadFromArrays(data.ToArray<object[]>());

                //Save Excel file
                excel.SaveAs(excelFile);
            }
        }

        private void WriteExelFileSkills(List<object[]> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage())
            {
                //Add Worksheets in Excel file
                excel.Workbook.Worksheets.Add("JobProfile Skills");

                //Create Excel file in Uploads folder of your project
                FileInfo excelFile = new FileInfo(Server.MapPath($"~/OrchardCoreExcelFiles/JobProfileSkills.xlsx"));

                //Add header row columns name in string list array
                var headerRow = new List<string[]>();

                headerRow = new List<string[]>()
                    {
                       new string[] { "Id", "Title", "SOC Code", "Skill-01", "Skill-02", "Skill-03", "Skill-04", "Skill-05", "Skill-06", "Skill-07", "Skill-08", "Skill-09", "Skill-10", "Skill-11", "Skill-12", "Skill-13", "Skill-14", "Skill-15", "Skill-16", "Skill-17", "Skill-18", "Skill-19", "Skill-20" }
                    };

                // Get the header range
                string range = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // get the workSheet in which you want to create header
                var worksheet = excel.Workbook.Worksheets["JobProfile Skills"];

                // Popular header row data
                worksheet.Cells[range].LoadFromArrays(headerRow);

                //show header cells with different style
                worksheet.Cells[range].Style.Font.Bold = true;
                worksheet.Cells[range].Style.Font.Size = 11;
                worksheet.Cells[range].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                //add the data in worksheet, here .Cells[2,1] 2 is rowNumber while 1 is column number
                //worksheet.Cells[2, 1].LoadFromCollection(data);
                worksheet.Cells[2, 1].LoadFromArrays(data.ToArray<object[]>());

                //Save Excel file
                excel.SaveAs(excelFile);
            }
        }
        #endregion Private Methods - WriteExelFile

        #region Private Methods - DynamicContentTypes - FilteringQuestions

        private List<OcFilteringQuestion> GetFilteringQuestions()
        {
            var filteringQuestions = dynamicModuleRepository.GetFilteringQuestions().ToList();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            var jsonData = JsonConvert.SerializeObject(filteringQuestions, jsonSerializerSettings);
            var fullPathAndFileName = JsonFilePath + OcItemTypes.PersonalityFilteringQuestion + "-" + filteringQuestions.Count().ToString() + ".json";
            System.IO.File.WriteAllText(fullPathAndFileName, RecipeBeginningCSharpContent + jsonData + RecipeEnd);
            return filteringQuestions;
        }

        #endregion Private Methods -  - DynamicContentTypes - FilteringQuestions
    }
}