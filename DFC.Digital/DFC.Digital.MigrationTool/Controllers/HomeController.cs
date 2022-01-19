using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.MigrationTool.Models.Home;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DFC.Digital.MigrationTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IDynamicModuleRepository<JobProfile> dynamicModuleJobProfileRepository;

        public HomeController(IJobProfileRepository jobProfileRepository,
            IDynamicModuleRepository<JobProfile> dynamicModuleJobProfileRepository) 
        {
            this.jobProfileRepository = jobProfileRepository;
            this.dynamicModuleJobProfileRepository = dynamicModuleJobProfileRepository;
        }

        public HomeController()
        {

        }

        public string CurrentJobProfileUrl { get; private set; } = "nanny";

        public ActionResult Index()
        {
            var model = new HomeViewModel();
            //var allJobProfiles = dynamicModuleJobProfileRepository.GetAll();
            var currentJobProfile = jobProfileRepository.GetByUrlName(CurrentJobProfileUrl);
            model.CurrentJobProfile = currentJobProfile;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}