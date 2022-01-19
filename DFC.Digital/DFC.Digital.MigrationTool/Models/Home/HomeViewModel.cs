using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.MigrationTool.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            CurrentJobProfile = new JobProfile();
        }

        public JobProfile CurrentJobProfile { get; set; }
    }
}