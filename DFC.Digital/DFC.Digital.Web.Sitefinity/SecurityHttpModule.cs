using System;
using System.Web;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity
{
    public sealed class SecurityHttpModule : IHttpModule
    {
        private readonly string contentSecurityPolicy = "Content-Security-Policy";

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PreSendRequestHeaders += App_PreSendRequestHeaders;
        }

        private void App_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var context = SystemManager.CurrentHttpContext;

            if (context == null || context.Response.HeadersWritten)
            {
                return;
            }

            // Replace 'unsafe-inline' & 'unsafe-eval'
            if (context.Items != null
                && ((context.Items[SystemManager.IsBackendRequestKey] != null && (!(bool)context.Items[SystemManager.IsBackendRequestKey]))
                || context.Items[SystemManager.IsBackendRequestKey] == null))
            {
                var header = context.Response.Headers[contentSecurityPolicy];
                if (header != null)
                {
                    context.Response.Headers.Remove(contentSecurityPolicy);

                    header = header.Replace("'unsafe-inline'", string.Empty).Replace("'unsafe-eval'", string.Empty);
                    context.Response.Headers.Add(contentSecurityPolicy, header);
                }
            }
        }
    }
}