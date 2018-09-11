using System.Linq;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.SkillsFramework.UnitTests
{
    public class InMemoryReportAuditRepositoryTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetAllAuditRecordsTests(bool isEmpty)
        {
            var objectToTest = new InMemoryReportAuditRepository();

            if (isEmpty)
            {
                objectToTest.CreateAudit("test", "dummy");
            }
        
           objectToTest.GetAllAuditRecords().Keys.Any().Should().Be(isEmpty);
            
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateAuditTests(bool categoryExists)
        {
            var objectToTest = new InMemoryReportAuditRepository();

            if (categoryExists)
            {
                objectToTest.CreateAudit("test", "dummy");
            }

            objectToTest.CreateAudit("test", "test");

             objectToTest.GetAllAuditRecords()["test"].Should().Contain("test");
            
        }
    }
}
