using System;
using System.Collections.Generic;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    public class HelperEfOnetDatas
    {
        public static readonly  IEnumerable<DFC_SocMappings> DfcSocMappings = new List<DFC_SocMappings>
        {
            new DFC_SocMappings
            {
                ONetCode = "11-3031.02",
                SocCode = "1150",

            },
            new DFC_SocMappings
            {
                ONetCode = "11-9051.00",
                SocCode = "1223",

            },
            new DFC_SocMappings
            {
                ONetCode = "19-4092.00",
                SocCode = "2112",

            },
            new DFC_SocMappings
            {
                ONetCode = "17-2141.00",
                SocCode = "2122",

            },

        };
        public static readonly IEnumerable<DFC_GDSTranlations> DfcTranslations = new List<DFC_GDSTranlations>
        {

            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.a",
                translation = "excellent verbal communication skills and mathematics knowledge"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1a",
                translation = "problem-solving skills and working characterstics"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.b",
                translation = "thinking and reasoning skills and adventourus workstlye"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.c",
                translation = "maths skills  knowledge and algorithms knowledge"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.d",
                translation = "a good memory and mathematics solving skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.e",
                translation = "great leadership and behavioural characterstics"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.g",
                translation = "concentration skills"
            },

            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.h",
                translation = "the ability to work well with your hands"
            },

        };
        public static readonly IEnumerable<DFC_GDSCombinations> DfcCombination = new List<DFC_GDSCombinations>
        {
            new DFC_GDSCombinations()
            {
                application_order = 1,
                combined_element_id = "C1",
                datetimestamp = DateTime.Today,
                description = "Hello Unit Test",
                element_name = "Hello Unit1",
                onet_element_one_id = "1.A.1.a",
                onet_element_two_id = "1.A.1a",
            },
            new DFC_GDSCombinations()
            {
                application_order = 2,
                combined_element_id = "C2",
                datetimestamp = DateTime.Today,
                description = "Hello Unit Test 2" ,
                element_name = "Hello Unit2",
                onet_element_one_id = "1.A.1.b",
                onet_element_two_id = "1.A.1.c",
            },

        };
        public static readonly IEnumerable<tools_and_technology> DfcToolsandTechnology = new List<tools_and_technology>
        {
            new tools_and_technology()
            {
                commodity_code = 44101805,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.03",
                t2_type = "Tools",
                t2_example = "Laptop small"
            },
            new tools_and_technology()
            {
                commodity_code = 44101806,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.04",
                t2_type = "Tools",
                t2_example = "Laptop big"
            },
            new tools_and_technology()
            {
                commodity_code = 44101807,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.05",
                t2_type = "Tools",
                t2_example = "Laptop medium"
            },
            new tools_and_technology()
            {
                commodity_code = 44101808,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.06",
                t2_type = "Technology",
                t2_example = "tablet C"
            },
            new tools_and_technology()
            {
                commodity_code = 44101819,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.07",
                t2_type = "Technology",
                t2_example = "Laptop B"
            },

        };
        public static readonly IEnumerable<content_model_reference> OnetContentModelReference =
            new List<content_model_reference>()
            {
                new content_model_reference()
                {
                    element_id = "1.A.1.a",
                    element_name = "Mathematics",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1a",
                    element_name = "abilties",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.b",
                    element_name = "workstyle",
                    description = "Workestyle Characteristics aa"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.c",
                    element_name = "knowledge",
                    description = "Worker Characteristics a"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.d",
                    element_name = "Mathematics",
                    description = "skills b"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.e",
                    element_name = "skills 2",
                    description = "Worker Characteristics d"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.g",
                    element_name = "skills 3",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.h",
                    element_name = "skills 4",
                    description = "Worker Characteristics e"
                },
            };
    }
}