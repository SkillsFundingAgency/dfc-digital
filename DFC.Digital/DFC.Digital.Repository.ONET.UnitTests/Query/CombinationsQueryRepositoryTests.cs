using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Query;


namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class CombinationsQueryRepositoryTests
    {
              
        [Fact]
        public void GetAllCombinationsTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new CombinationsQueryRepository(fakeDbContext);

            var fakeSuppressionsDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                   .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                   .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                   .SetupData(GetTestCombinationsTableData());

            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeSuppressionsDbSet);
          
            //Act
            var result = repo.GetAll().ToList();

            //Assert
            result.Count().Should().Be(2);

            //the result should have be ordered on application_order decending
            result[0].CombinedElementId.Should().Be("C2");
        }

        private List<DFC_GDSCombinations> GetTestCombinationsTableData()
        {
            List<DFC_GDSCombinations> testData = new List<DFC_GDSCombinations>();
            testData.Add(new DFC_GDSCombinations { element_name = "Combination1", combined_element_id = "C1", onet_element_one_id = "O1", onet_element_two_id = "02", application_order = 2});
            testData.Add(new DFC_GDSCombinations { element_name = "Combination2", combined_element_id = "C2", onet_element_one_id = "O21", onet_element_two_id = "022", application_order = 1 });
            return testData;
        }
    }
}