﻿using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class CourseDetails : Course
    {
        public Venue VenueDetails { get; set; }

        public ProviderDetails ProviderDetails { get; set; }

        public IList<Oppurtunity> Oppurtunities { get; set; }

        public string Description { get; set; }

        public string EntryRequirements { get; set; }

        public string QualificationName { get; set; }

        public string AssessmentMethod { get; set; }

        public string Cost { get; set; }

        public string EquipmentRequired { get; set; }

        public string AwardingOrganisation { get; set; }

        public string SubjectCategory { get; set; }

        public string CourseWebpageLink { get; set; }

        public string AdditionalPrice { get; set; }

        public string SupportingFacilities { get; set; }

        public string LanguageOfInstruction { get; set; }

        public string NextSteps { get; set; }

        public string WhatYoullLearn { get; set; }

        public string HowYoullLearn { get; set; }

        public IList<CourseRegion> CourseRegions { get; set; }

        public bool National { get; set; }
    }
}
