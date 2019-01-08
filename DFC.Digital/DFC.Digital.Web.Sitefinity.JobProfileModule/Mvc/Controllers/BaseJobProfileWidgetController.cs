using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    public abstract class BaseJobProfileWidgetController : BaseDfcController
    {
        private const string AcronymPattern = ".*([A-Z]\\W*[A-Z]).*";

        private readonly IJobProfileRepository jobProfileRepository;
        private JobProfile jobProfile;
        private ISitefinityPage sitefinityPage;

        protected BaseJobProfileWidgetController(IWebAppContext webAppContext, IJobProfileRepository jobProfileRepository, IApplicationLogger loggingService, ISitefinityPage sitefinityPage) : base(loggingService)
        {
            this.WebAppContext = webAppContext;
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

        protected IWebAppContext WebAppContext { get; set; }

        protected JobProfile CurrentJobProfile
        {
            get
            {
                if (jobProfile == null)
                {
                    jobProfile = WebAppContext.IsContentPreviewMode ? jobProfileRepository.GetByUrlNameForPreview(CurrentJobProfileUrl) : jobProfileRepository.GetByUrlName(CurrentJobProfileUrl);
                }

                return jobProfile;
            }
        }

        public string GetDynamicTitle(bool skipNoTitle)
        {
            var changedTitle = CheckForAcronym(CurrentJobProfile.Title);
            switch (CurrentJobProfile.DynamicTitlePrefix)
            {
                case "No Prefix":
                    return $"{changedTitle}";

                case "Prefix with a":
                    return $"a {changedTitle}";

                case "Prefix with an":
                    return $"an {changedTitle}";

                case "No Title":
                    return skipNoTitle ? GetDefaultDynamicTitle(changedTitle) : string.Empty;

                default:
                    return GetDefaultDynamicTitle(changedTitle);
            }
        }

        public string CheckForAcronym(string title)
        {
            return title.Split(' ').Aggregate(string.Empty, (current, next) => $"{current} {(IsSpecialConditionWords(next) ? next : ChangeWordCase(next))}").Trim();
        }

        // Further investigation on implmenting Special Title Case for certain JobTitles will be done in the story - 6426
        // https://skillsfundingagency.atlassian.net/browse/DFC-6426
        public bool IsSpecialConditionWords(string word)
        {
            var specialConditionWords = new[]
            {
                "European",
                "Ofsted",
                "Royal",
                "Marines",
                "Navy",
                "Merchant",
                "Montessori",
                "Portage",
                "Post",
                "Office",
                "Civil",
                "Service",
                "Border",
                "Force",
                "Union",
            };

            return specialConditionWords.Any(s => word.StartsWith(s, StringComparison.OrdinalIgnoreCase));
        }

        public string ChangeWordCase(string word)
        {
            return Regex.IsMatch(word, AcronymPattern) ? word : word.ToLower();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Redirect</returns>
        public ActionResult BaseIndex()
        {
            if (WebAppContext.IsContentAuthoringSite)
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
        /// <param name="urlName">The urlname.</param>
        /// <returns>Action Result</returns>
        public ActionResult BaseIndex(string urlName)
        {
            CurrentJobProfileUrl = urlName;
            if (CurrentJobProfile is null)
            {
                return HttpNotFound();
            }
            else
            {
                var timer = Stopwatch.StartNew();
                WebAppContext.SetMetaDescription(CurrentJobProfile.Overview);
                var actionResult = GetDefaultView();
                timer.Stop();
                Log.Trace($"Completed executing action in {timer.Elapsed}");

                return actionResult;
            }
        }

        protected abstract ActionResult GetDefaultView();

        protected abstract ActionResult GetEditorView();

        private static string GetDefaultDynamicTitle(string title) => IsStartsWithVowel(title) ? $"an {title}" : $"a {title}";

        private static bool IsStartsWithVowel(string title) => new[] { 'a', 'e', 'i', 'o', 'u' }.Contains(title.ToLower().First());
    }
}