using Xunit;
using DFC.Digital.Repository.ONET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DFC.Digital.Repository.ONET.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Data.Model;
    using DataModel;
    using DFC.Digital.Repository.ONET.Mapper;
    using FakeItEasy;
    using FluentAssertions;
    using Helper;
    using Impl;
    using Model;
    public class OnetRepositoryTest:HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(SocMappings))]
        public void GetAllSocMappingsAsyncTest(List<DfcOnetSocMappings> socMappings)
        {
            //Arrange
            var applicationLogger = A.Fake<IApplicationLogger>();
            var fakeIQueryable = new List<DFC_SocMappings>().AsQueryable();
            var context = A.Fake<IObjectContextFactory<OnetRepositoryDbContext>>();
            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>((d => d.Implements(typeof(IQueryable<DFC_SocMappings>))));
            //Act
            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).GetEnumerator()).Returns(fakeIQueryable.GetEnumerator());
            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).Provider).Returns(fakeIQueryable.Provider);
            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).Expression).Returns(fakeIQueryable.Expression);
            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).ElementType).Returns(fakeIQueryable.ElementType);
            // A.CallTo(() => onetSkillsRepository.GetAllSocMappingsAsync<DfcOnetSocMappings>()).Returns(socMappings);
            A.CallTo(() => context.GetContext().DFC_SocMappings).Returns(fakeDbSet);
          //  A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            //Assert
            var onetRespository = new OnetRepository(context,iMapper,applicationLogger);
            var response = onetRespository.GetAllSocMappingsAsync<DfcOnetSocMappings>();

          //  A.CallTo(() => context.GetContext().Set<DFC_SocMappings>()).MustHaveHappened();
         //   A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustNotHaveHappened();

            //response.Result.Should().NotBeNull();
            //response.Result.Should().BeSameAs(socMappings);
            onetRespository.Dispose();
        }

        [Fact()]
        public void GetAllTranslationsAsyncTest()
        {
           // Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetAttributesValuesAsyncTest()
        {
          //  Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetDigitalSkillsAsyncTest()
        {
          //  Xunit.Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetDigitalSkillsRankAsyncTest()
        {
          //  Xunit.Assert.True(false, "This test needs an implementation");
        }
    }
}
