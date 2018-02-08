using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    public abstract class BaseJobProfileWidgetController : BaseDfcController
    {
        private readonly IWebAppContext webAppContext;
        private readonly IJobProfileRepository jobProfileRepository;
        private JobProfile jobProfile;
        private ISitefinityPage sitefinityPage;

        protected BaseJobProfileWidgetController(IWebAppContext webAppContext, IJobProfileRepository jobProfileRepository, IApplicationLogger loggingService, ISitefinityPage sitefinityPage) : base(loggingService)
        {
            this.webAppContext = webAppContext;
            this.jobProfileRepository = jobProfileRepository;
            this.sitefinityPage = sitefinityPage;
        }

        public string CurrentJobProfileUrl { get; private set; }

        /// <summary>
        /// Gets or sets the default name of the job URL.
        /// </summary>
        /// <value>
        /// The default name of the job URL.
        /// </value>
        public string DefaultJobProfileUrlName { get; set; } = "plumber";

        protected JobProfile CurrentJobProfile
        {
            get
            {
                if (jobProfile == null)
                {
                    jobProfile = webAppContext.IsContentPreviewMode ? jobProfileRepository.GetByUrlNameForPreview(CurrentJobProfileUrl) : jobProfileRepository.GetByUrlName(CurrentJobProfileUrl);
                }

                return jobProfile;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Redirect</returns>
        public ActionResult BaseIndex()
        {
            if (webAppContext.IsContentAuthoringSite)
            {
                CurrentJobProfileUrl = sitefinityPage.GetDefaultJobProfileToUse(DefaultJobProfileUrlName);
                return GetEditorView();
            }
            else
            {
                return Redirect("\\");
            }
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlname">The urlname.</param>
        /// <returns>Action Result</returns>
        public ActionResult BaseIndex(string urlname)
        {
            CurrentJobProfileUrl = urlname;
            if (CurrentJobProfile == null)
            {
                return HttpNotFound();
            }
            else
            {
                return GetDefaultView();
            }
        }

        protected abstract ActionResult GetDefaultView();

        protected abstract ActionResult GetEditorView();
    }
}