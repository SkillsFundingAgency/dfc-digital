using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using Xunit;
namespace DFC.Digital.Service.LMIFeed.UnitTests
{
    using Model;

    public class SalaryCalculatorTests :HelperJobProfileData
    {
        [Theory]
        [MemberData(nameof(StarterSalaryMedianDeciles))]
        public void GetStarterSalaryTest(IEnumerable<KeyValuePair<int, decimal>> medianDecile,decimal expectedStarterSalary)
        {
            //Act
            ISalaryCalculator salaryCalculator = new SalaryCalculator();

            //Arrange 
            var keyValuePairs = medianDecile as KeyValuePair<int, decimal>[] ?? medianDecile.ToArray();
            var starterSalary = salaryCalculator.GetStarterSalary(new JobProfileSalary() { Deciles = (keyValuePairs.ToDictionary(x => x.Key, x => x.Value)) });

            //Assert
            starterSalary.Should().Be(expectedStarterSalary);
        }

        [Theory]
        [MemberData(nameof(ExperiencedSalaryMedianDeciles))]
        public void GetExperiencedSalaryTest(IEnumerable<KeyValuePair<int, decimal>> medianDecile,decimal expectedExperiencedSalary)
        {
            //Act
            ISalaryCalculator salaryCalculator = new SalaryCalculator();

            //Arrange 
            var keyValuePairs = medianDecile as KeyValuePair<int, decimal>[] ?? medianDecile.ToArray();
            var starterSalary = salaryCalculator.GetExperiencedSalary(new JobProfileSalary() {Deciles = (keyValuePairs.ToDictionary(x=>x.Key,x=>x.Value))});

            //Assert
            starterSalary.Should().Be(expectedExperiencedSalary);

        }
    }
}