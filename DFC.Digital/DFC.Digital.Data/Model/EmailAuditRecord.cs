using System;

namespace DFC.Digital.Data.Model
{
    public class EmailAuditRecord<T>
    {
        public Exception Exception { get; set; }

        public T Request { get; set; }

        public SendEmailResponse SendEmailResponse { get; set; }

        public EmailTemplate EmailTemplate { get; set; }

        public string EmailContent { get; set; }
    }
}
