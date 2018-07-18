using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DFC.Digital.Service.SkillsFramework.Tests
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Data.Model;
    using DFC.Digital.Core;
    using DFC.Digital.Repository.ONET.Interface;
    using FakeItEasy;
    using FluentAssertions;
    using Interface;
    using Repository.ONET;
    using Repository.ONET.Helper;
    using Repository.ONET.Impl;
    using Repository.ONET.Mapper;
    using UnitTests.Model;
    public class SkillsFrameworkTest:HelperOnetDatas
    {
        [Fact()]
        public void GetAllTranslationsAsyncTest()
        {
            Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Theory]
        [MemberData(nameof(SocMappings))]
        public void GetAllSocMappingsAsyncTest(List<DfcGdsSocMappings> socMappings)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var onetSkillsRepository = A.Fake<IOnetSkillsFramework>();

            //Act
            A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<DfcGdsSocMappings>()).Returns(socMappings);
            A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            IBusinessRuleEngine ruleEngine =new SkillsFrameworkEngine(onetSkillsRepository,applicationLogger);

            //Assert
            var response = ruleEngine.GetAllSocMappingsAsync();
            A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<DfcGdsSocMappings>()).MustHaveHappened();
            response.Result.Should().BeSameAs(socMappings);

        }

        [Fact()]
        public void GetAllDigitalSkillsAsyncTest()
        {
            Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetDigitalSkillRankAsyncTest()
        {
            Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetBusinessRuleAttributesAsyncTest()
        {
            Xunit.Assert.True(false, "This test needs an implementation");
        }
    }
}

