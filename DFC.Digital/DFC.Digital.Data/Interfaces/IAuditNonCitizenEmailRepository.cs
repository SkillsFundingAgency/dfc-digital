﻿using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAuditNoncitizenEmailRepository<in T>
    {
        void CreateAudit(T emailRequest, EmailTemplate emailTemplate, SendEmailResponse response);
    }
}
