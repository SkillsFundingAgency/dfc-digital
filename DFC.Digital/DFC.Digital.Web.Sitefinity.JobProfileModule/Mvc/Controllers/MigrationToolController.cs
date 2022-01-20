using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
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

        private readonly IDynamicModuleRepository<JobProfile> dynamicModuleJobProfileRepository;
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository;

        #endregion Private Fields

        #region Constructors
        public MigrationToolController(
            IDynamicModuleRepository<JobProfile> dynamicModuleJobProfileRepository,
            IJobProfileRepository jobProfileRepository,
            IDynamicModuleRepository<SocCode> dynamicModuleSocCodeRepository)
        {
            this.dynamicModuleJobProfileRepository = dynamicModuleJobProfileRepository;
            this.jobProfileRepository = jobProfileRepository;
            this.dynamicModuleSocCodeRepository = dynamicModuleSocCodeRepository;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Current ContentType Name")]
        public string CurrentContentTypeName { get; set; }

        #endregion Public Properties

        #region Actions

        public ActionResult Index()
        {
            var model = new MigrationToolViewModel();

            model.JobProfiles = jobProfileRepository.GetAllJobProfiles().ToList();

            //var jobProfiles = new List<JobProfile>();
            //var jobProfilesDynamicContentItems = dynamicModuleJobProfileRepository.GetAll().ToList();
            //foreach(var jobProfilesDynamicContentItem in jobProfilesDynamicContentItems)
            //{
            //    JobProfile jobProfile = jobProfileRepository.co
            //}
            var socCode = dynamicModuleSocCodeRepository.GetAll().ToList();
            return View(model);
        }

        #endregion Actions
    }
}