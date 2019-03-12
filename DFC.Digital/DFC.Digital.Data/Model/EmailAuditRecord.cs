using System;

namespace DFC.Digital.Data.Model
{
    public class EmailAuditRecord
    {
        public Exception Exception { get; set; }

        public ContactUsRequest ContactUsRequest { get; set; }

        public SendEmailResponse SendEmailResponse { get; set; }

        public EmailTemplate EmailTemplate { get; set; }

        public string EmailContent { get; set; }
    }
}
