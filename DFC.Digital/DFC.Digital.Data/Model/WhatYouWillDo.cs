using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class WhatYouWillDo
    {
        public bool IsCadReady { get; set; }

        public string DailyTasks { get; set; }

        public IEnumerable<string> Locations { get; set; }

        public IEnumerable<string> Uniforms { get; set; }

        public IEnumerable<string> Environments { get; set; }

        public string Introduction { get; set; }
    }
}