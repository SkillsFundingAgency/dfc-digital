using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Data.Model
{
    public class JobProfileRelatedCareer : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ProfileLink { get; set; }
    }
}