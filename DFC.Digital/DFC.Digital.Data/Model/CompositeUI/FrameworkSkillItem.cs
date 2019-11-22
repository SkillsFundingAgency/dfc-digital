using DFC.Digital.Data.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace DFC.Digital.Data.Model
{
    public class FrameworkSkillItem : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ONetElementId { get; set; }

        public string Description { get; set; }
    }
}
