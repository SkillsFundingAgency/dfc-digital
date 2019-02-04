using Xunit;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using AutoMapper;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.UnitTests
{

    public class SocMappingsQueryRepositoryTests:HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(GetAllSocMappingData))]
        public void GetAllTest(IReadOnlyCollection<DFC_SocMappings> setupSocData, IReadOnlyCollection<SocCode> responseData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                .Implements(typeof(IQueryable<DFC_SocMappings>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetAll();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            if (responseData == null || result==null)
            {
                Assert.True(false, "Response Data should not be null");
            }
            else
            {
                result.Count().Should().Be(responseData.Count);
                result.Should().BeEquivalentTo(responseData);
                result.Should().Contain(x => x.SOCCode == setupSocData.First().SocCode);
            }

        }

        [Theory]
        [MemberData(nameof(GetSocMappingStatusData))]
        public void GetSocMappingStatusTest(IReadOnlyCollection<DFC_SocMappings> setupSocData, IReadOnlyList<int> itemCount)
        {
            if (itemCount == null)
            {
                throw new ArgumentNullException(nameof(itemCount));
            }

            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);

            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetSocMappingStatus();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Times(3));

            result.AwaitingUpdate.Should().Be(itemCount[0]);
            result.SelectedForUpdate.Should().Be(itemCount[1]);
            result.UpdateCompleted.Should().Be(itemCount[2]);
        }

        [Theory]
        [MemberData(nameof(GetSocsAwaitingUpdateData))]
        public void GetSocsAwaitingUpdateTest(IReadOnlyCollection<DFC_SocMappings> setupSocData, IQueryable<SocCode> responseData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);

            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetSocsAwaitingUpdate();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);

            result.Should().BeEquivalentTo(responseData);
        }

        [Theory]
        [MemberData(nameof(GetSocsSelectedForUpdateData))]
        public void GetSocsInStartedStateTest(IReadOnlyCollection<DFC_SocMappings> setupSocData, IQueryable<SocCode> responseData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);

            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetSocsInStartedState();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);

            result.Should().BeEquivalentTo(responseData);
        }

        [Theory]
        [InlineData(SkillsFrameworkUpdateStatus.UpdateCompleted)]
        [InlineData(SkillsFrameworkUpdateStatus.SelectedForUpdate)]
        [InlineData(SkillsFrameworkUpdateStatus.AwaitingUpdate)]
        public void SetUpdateStatusForSocsTest(SkillsFrameworkUpdateStatus status)
        {
            var setupSocData = MixedCombination();
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);

            A.CallTo(() => fakeDbContext.SaveChanges()).Returns(1);

            var repo = new SocMappingRepository(fakeDbContext, actualMapper);
            repo.SetUpdateStatusForSocs(new List<SocCode> {new SocCode { SOCCode = nameof(SocCode.SOCCode)}}, status);

            A.CallTo(() => fakeDbContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Theory]
        [MemberData(nameof(GetByIdSocMappingData))]
        public void GetByIdTest(IReadOnlyCollection<DFC_SocMappings> setupSocData,SocCode responseData,string socCode)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetById(socCode);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            if (responseData == null || result==null)
            {
                Assert.True(false, "Response Data should not be null");
            }
            else
            {
                result.Should().BeEquivalentTo(responseData);
            }
        }

        [Theory]
        [MemberData(nameof(GetByIdSocMappingData))]
        public void GetTest(IReadOnlyCollection<DFC_SocMappings> setupSocData, SocCode responseData, string socCode)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupSocData.ToList());


            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);


            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.Get(x=>x.SOCCode==socCode);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            if (responseData == null || result == null)
            {
                Assert.True(false, "Response Data should not be null");
            }
            else
            {
                result.Should().BeEquivalentTo(responseData);
            }
        }

        [Theory]
        [MemberData(nameof(GetManySocMappingData))]
        public void GetManyTest(IReadOnlyCollection<DFC_SocMappings> setupData, IReadOnlyCollection<SocCode> responseData, string colData1, string colData2)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var actualMapper = new MapperConfiguration(c => c.AddProfile<SkillsFrameworkMapper>()).CreateMapper();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))
                .SetupData(setupData.ToList());

            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);

            var repo = new SocMappingRepository(fakeDbContext, actualMapper);

            var result = repo.GetMany(x => x.SOCCode == colData1 || x.SOCCode==colData2);
            result.Count().Should().BeGreaterOrEqualTo(2);
            A.CallTo(() => fakeDbContext.DFC_SocMappings).MustHaveHappened(Repeated.Exactly.Once);
            if (responseData == null || result == null)
            {
                Assert.True(false, "Response Data should not be null");
            }
            else
            {
                result.Should().BeEquivalentTo(responseData);
            }
        }

        [Fact]
        public void AddNewSOCMappingsTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>(c => c
                    .Implements(typeof(IQueryable<DFC_SocMappings>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)));
            var fakeMapper = A.Fake<IMapper>();
            A.CallTo(() => fakeDbContext.DFC_SocMappings).Returns(fakeDbSet);
            A.CallTo(() => fakeDbContext.SaveChanges()).Returns(1);

            var repo = new SocMappingRepository(fakeDbContext, fakeMapper);
            var testSOC = new SocCode() { SOCCode = "TestSOC1", ONetOccupationalCode = "TestONetCode1", Description = "TestDescription" };
            var testSOCList = new List<SocCode>
            {
                testSOC
            };
 
            //Call
            repo.AddNewSOCMappings(testSOCList);

            //Asserts
            A.CallTo(() => fakeDbContext.SaveChanges()).MustHaveHappened(Repeated.Exactly.Once);
        }

    }
}