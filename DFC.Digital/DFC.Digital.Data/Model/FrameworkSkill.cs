using System;
using System.Text.RegularExpressions;

namespace DFC.Digital.Data.Model
{
    public class FrameworkSkill
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string OnetElementId { get; set; }

        public string Description { get; set; }

        public string SfUrlName => Regex.Replace(Title.ToLower().Trim(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
    }
}
