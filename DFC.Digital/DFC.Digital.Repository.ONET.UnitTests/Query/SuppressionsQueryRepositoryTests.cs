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
using DFC.Digital.Repository.ONET.Query;


namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class SuppressionsQueryRepositoryTests 
    {
              
        [Fact]
        public void GetAllTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new SuppressionsQueryRepository(fakeDbContext);

            var fakeSuppressionsDbSet = A.Fake<DbSet<DFC_GlobalAttributeSuppression>>(c => c
                   .Implements(typeof(IQueryable<DFC_GlobalAttributeSuppression>))
                   .Implements(typeof(IDbAsyncEnumerable<DFC_GlobalAttributeSuppression>)))
                   .SetupData(GetTestSuppressionTableData());

            A.CallTo(() => fakeDbContext.DFC_GlobalAttributeSuppression).Returns(fakeSuppressionsDbSet);
          
            //Act
            var result = repo.GetAll();

            //Assert
            result.Count().Should().Be(2);
        }

        private List<DFC_GlobalAttributeSuppression> GetTestSuppressionTableData()
        {
            List<DFC_GlobalAttributeSuppression> testData = new List<DFC_GlobalAttributeSuppression>();
            testData.Add(new DFC_GlobalAttributeSuppression { onet_element_id = "Suppress1" });
            testData.Add(new DFC_GlobalAttributeSuppression { onet_element_id = "Suppress2" });
            return testData;
        }
    }
}