using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Mapper;
using DFC.Digital.Repository.ONET.Tests.Model;

namespace DFC.Digital.Repository.ONET.Query.Tests
{

    public class TranslationQueryRepositoryTests : HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(OnetWhatitTakesData))]
        public void GetByIdTest(List<DFC_GDSTranlations> setupDbSetData, WhatItTakesSkill mappedWhatitTakesData, string onetElementId)
        {

            //InProgress as have to yield single object against collection
            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetById(onetElementId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedWhatitTakesData);
            result.Title.Should().Be(onetElementId);
        }

        [Theory]
        [MemberData(nameof(OnetWhatitTakesData))]
        public void GetTest(List<DFC_GDSTranlations> setupDbSetData, WhatItTakesSkill mappedWhatitTakesData, string onetElementId)
        {
            //InProgress as have to yield single object against collection
            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.Get(x => x.Title == onetElementId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedWhatitTakesData);
            result.Title.Should().Be(onetElementId);
        }

        [Theory]
        [MemberData(nameof(OnetTranslationsData))]
        public void GetAllTest(List<DFC_GDSTranlations> setupDbSetData, List<WhatItTakesSkill> mappedReturnDbSetData, string onetElementId)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetAll();
            result.Should().BeEquivalentTo(mappedReturnDbSetData);

        }

        [Theory]
        [MemberData(nameof(OnetWhatitTakesManyData))]
        public void GetManyTest(List<DFC_GDSTranlations> setupDbSetData, WhatItTakesSkill mappedWhatitTakesData, string onetElementId, string description)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetMany(x => x.Title == onetElementId && x.Description == description);
            result.Should().BeEquivalentTo(mappedWhatitTakesData);
        }
    }
}