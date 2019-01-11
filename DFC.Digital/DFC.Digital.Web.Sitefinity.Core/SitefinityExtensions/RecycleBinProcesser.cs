using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    public class RecycleBinProcesser
    {
        private readonly IRecycleBinRepository recycleBinRepository;
        private readonly IWebAppContext webAppContext;

        public RecycleBinProcesser(IRecycleBinRepository recycleBinRepository, IWebAppContext webAppContext)
        {
            this.recycleBinRepository = recycleBinRepository;
            this.webAppContext = webAppContext;
        }

        public void RunProcess(int itemCount)
        {
            webAppContext.CheckAuthenticationByAuthCookie();
            recycleBinRepository.DeleteVacanciesPermanently(itemCount);
        }
    }
}