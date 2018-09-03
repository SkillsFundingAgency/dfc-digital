namespace DFC.Digital.Web.Sitefinity.BreadCrumbs
{
    using System.Web;

    public class HttpSessionProvider : IProvideBreadCrumbsSession
	{
		public string SessionId
		{
			get
			{
				var id = HttpContext.Current.Session.SessionID;
				var sessionKey = string.Format("{0}-SessionId.MvcBreadCrumbs", id);
                HttpContext.Current.Session[sessionKey] = id;
				return id;
			}
		}
	}
}
