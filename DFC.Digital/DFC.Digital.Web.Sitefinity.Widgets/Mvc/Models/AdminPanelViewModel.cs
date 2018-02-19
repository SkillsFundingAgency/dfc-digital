using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class AdminPanelViewModel
    {
        public string PageTitle { get; set; }

        public string FirstParagraph { get; set; }

        public string NotAllowedMessage { get; set; }

        public bool IsAdmin { get; set; }
    }
}