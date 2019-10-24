using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class WhatYouWillDoData
    {
        public bool IsCadReady { get; set; }

        public string DailyTasks { get; set; }

        public IEnumerable<WYDRelatedContentType> Locations { get; set; }

        public IEnumerable<WYDRelatedContentType> Uniforms { get; set; }

        public IEnumerable<WYDRelatedContentType> Environments { get; set; }

        public string Introduction { get; set; }
    }
}