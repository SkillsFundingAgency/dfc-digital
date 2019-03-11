using System;

namespace DFC.Digital.Data.Model
{
    public class EmailAuditRecord
    {
        public Exception Exception { get; set; }

        public ContactAdvisorRequest ContactAdvisorRequest { get; set; }

        public SendEmailResponse SendEmailResponse { get; set; }

        public string EmailContent { get; set; }
    }
}
