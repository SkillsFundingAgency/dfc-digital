using System.Net;
using System.Net.Http.Headers;

namespace DFC.Digital.Data.Model
{
    public class SendEmailResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpResponseHeaders Headers { get; set; }
    }
}
