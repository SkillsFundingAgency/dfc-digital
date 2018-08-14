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
    public class ContentReferenceQueryRepositoryTests
    {

        [Fact]
        public void GetAllContentReferenceTest()
        {
            //Setup
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var repo = new ContentReferenceQueryRepository(fakeDbContext);
            var contentData = GetContentTableData();

            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                   .Implements(typeof(IQueryable<content_model_reference>))
                   .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                   .SetupData(contentData);

            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);

            //Act
            var result = repo.GetAll();

            //Assert
            int index = 0;
            foreach (var r in result)
            {
                r.ONetElementId.Should().Be(contentData[index].element_id);
                r.Title.Should().Be(contentData[index++].element_name);
            }
        }

        private List<content_model_reference> GetContentTableData()
        {
            List<content_model_reference> testData = new List<content_model_reference>();
            testData.Add(new content_model_reference { element_id = "E1", element_name = "EName1" });
            testData.Add(new content_model_reference { element_id = "E2", element_name = "EName2"});
            return testData;
        }
    }
}