﻿using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class SkillsFrameworkImportViewModel
    {
        public string PageTitle { get; set; }

        public string FirstParagraph { get; set; }

        public string NotAllowedMessage { get; set; }

        public bool IsAdmin { get; set; }

        public SocMappingStatus SocMappingStatus { get; set; }

        public string NextBatchOfSOCsToImport { get; set; }
    }
}