//using Xunit;
//using DFC.Digital.Repository.ONET;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace DFC.Digital.Repository.ONET.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data.Entity;
//    using System.Data.Entity.Infrastructure;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using AutoMapper;
//    using Core;
//    using Data.Model;
//    using DataModel;
//    using DFC.Digital.Repository.ONET.Mapper;
//    using FakeItEasy;
//    using FluentAssertions;
//    using Helper;
//    using Impl;
//    using Model;
//    public class OnetRepositoryTest:HelperOnetDatas
//    {
//        [Theory]
//        [MemberData(nameof(SocMappings))]
//        public async Task GetAllSocMappingsAsyncTest(List<DfcOnetSocMappings> socMappings)
//        {
//            List<DfcOnetSocMappings> socList = new List<DfcOnetSocMappings>()
//            {
//                new DfcOnetSocMappings()
//                {
//                    ElementId = "1111",
//                    JobProfile = "Police officer",
//                    OnetSocCode = "111-111",
//                    QualityRating = 4,
//                    SocCode = "1120"
//                }
//            };
//            var socMappingList = new List<DFC_SocMappings>()
//            {
//                new DFC_SocMappings()
//                {
//                    JobProfile = "Police Officer",
//                    ONetCode = "111-00.01",
//                    QualityRating = 4,
//                    SocCode = "1120"
//                },
//                new DFC_SocMappings()
//                {
//                    JobProfile = "Police Officer",
//                    ONetCode = "111-00.02",
//                    QualityRating = 4,
//                    SocCode = "1121"
//                },
//            };
//            var soc = new DFC_SocMappings()
//            {
//                JobProfile = "Police Officer",
//                ONetCode = "111-00.01",
//                QualityRating = 4,
//                SocCode = "1120"
//            };
//            //Arrange
//            var applicationLogger = A.Fake<IApplicationLogger>();
//            var fakeIQueryable = new List<DFC_SocMappings>().AsQueryable();
//            var context = A.Fake<IObjectContextFactory<OnetRepositoryDbContext>>();
            
//           // var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>();
//            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
//            var fakeDbSet = A.Fake<DbSet<DFC_SocMappings>>((d => d.Implements(typeof(IQueryable<DFC_SocMappings>)).Implements(typeof(IDbAsyncEnumerable<DFC_SocMappings>)))).SetupData(socMappingList);

//            A.CallTo(() => context.GetContext().DFC_SocMappings).Returns(fakeDbSet);

//            var ct=A.Fake<OnetRepository>(op=>op.WithArgumentsForConstructor(new object[]{context,iMapper,applicationLogger}));
//            //Act
//            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).GetEnumerator()).Returns(fakeIQueryable.GetEnumerator());
//            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).Provider).Returns(fakeIQueryable.Provider);
//            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).Expression).Returns(fakeIQueryable.Expression);
//            A.CallTo(() => ((IQueryable<DFC_SocMappings>)fakeDbSet).ElementType).Returns(fakeIQueryable.ElementType);
//            A.CallTo(() => ct.GetAllSocMappingsAsync<DfcOnetSocMappings>()).Returns(socList);
           
         
            
//            //Assert
//            var onetRespository = new OnetRepository(context,iMapper,applicationLogger);
//            var response = await ct.GetAllSocMappingsAsync<DfcOnetSocMappings>();
//            A.CallTo(() => context.GetContext().Set<DFC_SocMappings>()).MustHaveHappened();
//            var res = response;

//            A.CallTo(() => context.GetContext().Set<DFC_SocMappings>()).MustHaveHappened();
//         //   A.CallTo(() => applicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustNotHaveHappened();

//            response.Should().NotBeNull();
//            response.Should().BeSameAs(socMappings);
//            onetRespository.Dispose();
//        }

//        [Fact()]
//        public void GetAllTranslationsAsyncTest()
//        {
//           // Xunit.Assert.True(false, "This test needs an implementation");
//        }

//        [Fact()]
//        public void GetAttributesValuesAsyncTest()
//        {
//          //  Xunit.Assert.True(false, "This test needs an implementation");
//        }

//        [Fact()]
//        public void GetDigitalSkillsAsyncTest()
//        {
//          //  Xunit.Assert.True(false, "This test needs an implementation");
//        }

//        [Fact()]
//        public void GetDigitalSkillsRankAsyncTest()
//        {
//          //  Xunit.Assert.True(false, "This test needs an implementation");
//        }
//    }
//}
