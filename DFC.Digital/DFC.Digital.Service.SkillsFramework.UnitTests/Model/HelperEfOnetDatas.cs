using System;
using System.Collections.Generic;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    using System.Linq;

    public class HelperEfOnetDatas
    {
        public HelperEfOnetDatas()
        {
        }


        internal static readonly IEnumerable<DFC_SocMappings> DfcSocMappings = new List<DFC_SocMappings>
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
        internal static readonly IEnumerable<DFC_GDSTranlations> DfcTranslations = new List<DFC_GDSTranlations>
        {
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1",
                translation = "problem-solving skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.a",
                translation = "excellent verbal communication skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.b",
                translation = "thinking and reasoning skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.c",
                translation = "maths skills "
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.d",
                translation = "a good memory"
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
                onet_element_id = "1.A.2",
                translation = "physcial skills like movement, coordination, dexterity and grace"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.2.a",
                translation = "the ability to work well with your hands"
            },

        };
        internal static readonly IEnumerable<tools_and_technology> DfcToolsandTechnology = new List<tools_and_technology>
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
        internal static readonly IEnumerable<unspsc_reference> DfcUnspcsReference = new List<unspsc_reference>
        {
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101805,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101806,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101807,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101808,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101819,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            }
        };
        internal static readonly IEnumerable<content_model_reference> OnetContentModelReference =
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
                    element_name = "skills",
                    description = "Worker Characteristics d"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.g",
                    element_name = "skills",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.h",
                    element_name = "skills ",
                    description = "Worker Characteristics e"
                },
            };
        internal static readonly IEnumerable<knowledge> OnetKnowledges = new List<knowledge>()
        {
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[1],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
            content_model_reference = OnetContentModelReference.ToList()[2],
            data_value = (decimal)4.7,
            onetsoc_code = "11-1011.00",
            element_id = "1.A.1.a",
            not_relevant = "N",
            recommend_suppress = "N",
            occupation_data = new occupation_data(),
            scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[3],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[4],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[5],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            }
        };
        internal static readonly IEnumerable<skill> OnetSkills = new List<skill>
        {
            new skill()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                onetsoc_code = "11-1011.00",
                scale_id = "IM"
            },
            new skill()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                not_relevant = "N",
                recommend_suppress = "N",
                onetsoc_code = "11-1011.00",
                scale_id = "LV"
            },
        };
        internal static readonly IEnumerable<ability> OnetAbilities = new List<ability>()
        {
            new ability()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                scale_id = "IM",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
            new ability()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                not_relevant = "N",
                recommend_suppress = "N",
                scale_id = "LV",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
        };
        internal static readonly IEnumerable<work_styles> OnetWorkStyles = new List<work_styles>()
        {
            new work_styles()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                recommend_suppress = "N",
                scale_id = "IM",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
            new work_styles()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                recommend_suppress = "N",
                scale_id = "LV",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
        };
    }
}