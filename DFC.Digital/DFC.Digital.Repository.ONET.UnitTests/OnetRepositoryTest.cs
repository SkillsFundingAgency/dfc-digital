﻿using DFC.Digital.Repository.ONET.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Mapper;
using FakeItEasy;
using DFC.Digital.Repository.ONET.Impl;
using Xunit;
using DFC.Digital.Core;
using System;
using DFC.Digital.Repository.ONET.Helper;

namespace DFC.Digital.Repository.ONET.UnitTests
{


    public enum RangeChecker
    {
       FirstRange =150,
       SecondRange=100,
       ThirdRange=50,
       FourthRange=0
    }
    [TestClass]
    public class OnetRepositoryTest
    {
        [Fact]
        public void GetAllTransalations ( )
        {

            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<SkillsFrameworkDbContext> contextFactory=new ObjectContextFactory<SkillsFrameworkDbContext>();
            var mapper=A.Fake<IMapper>();
            using(IDfcGdsSkillsFramework repository = new OnetRepository(contextFactory, iMapper,appLogger))
            {
              var all=  repository.GetAllTranslationsAsync<DfcGdsTranslation>().Result;

            }
        }

        [Fact]
        public void GetAllSocMappings()
        {
            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var mapper = A.Fake<IMapper>();
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<SkillsFrameworkDbContext> contextFactory = new ObjectContextFactory<SkillsFrameworkDbContext>();

            using(IDfcGdsSkillsFramework repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var all = repository.GetAllSocMappingsAsync<DfcGdsSocMappings>().Result;

            }
        }

        [Fact]
        public void GetDigitalSkillRanks()
        {
            //Ranks >150 - return 1
            // >100 and <150 - return 2 
            //>50 and <100 -return 3
            //>0 and <50 -return 4

            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var mapper = A.Fake<IMapper>();
            var GetRank = 0;
            var onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<SkillsFrameworkDbContext> contextFactory = new ObjectContextFactory<SkillsFrameworkDbContext>();

            using(IDfcGdsSkillsFramework repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var rankResult = repository.GetDigitalSkillsRankAsync<int>(onetCode).Result;
                if (rankResult > Convert.ToInt32(RangeChecker.FirstRange))
                    GetRank = 1;
                if((rankResult > Convert.ToInt32(RangeChecker.SecondRange)) && (rankResult < Convert.ToInt32(RangeChecker.FirstRange)))
                    GetRank = 2;
                if((rankResult > Convert.ToInt32(RangeChecker.ThirdRange)) && (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
                    GetRank = 3;
                if((rankResult > Convert.ToInt32(RangeChecker.FourthRange)) && (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
                    GetRank = 4;

            }
        }

        [Fact]
        public void GetDigitalSkills()
        {
            //Ranks >150 - return 1
            // >100 and <150 - return 2
            //>50 and <100 -return 3
            //>0 and <50 -return 4

            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var mapper = A.Fake<IMapper>();
            var GetRank = 0;
            var onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<SkillsFrameworkDbContext> contextFactory = new ObjectContextFactory<SkillsFrameworkDbContext>();

            using(IDfcGdsSkillsFramework repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var digitalSkillsData = repository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(onetCode).Result;
                foreach (var skill in digitalSkillsData.DigitalSkillsCollection)
                {
                    var title = skill.ClassTitle;
                    var example = skill.T2Example;
                    var type = skill.T2Type;
                    var count = digitalSkillsData.DigitalSkillsRank;

                }



            }
        }

        [Fact]
        public void GetAttributes()
        {
            IMapper iMapper =
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var GetRank = 0;
            var onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<SkillsFrameworkDbContext> contextFactory = new ObjectContextFactory<SkillsFrameworkDbContext>();

            using(IDfcGdsSkillsFramework repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var digitalSkillsData = repository.GetAttributesValuesAsync<DfcGdsAttributesData>(onetCode).Result;

            }
        }
    }
}
