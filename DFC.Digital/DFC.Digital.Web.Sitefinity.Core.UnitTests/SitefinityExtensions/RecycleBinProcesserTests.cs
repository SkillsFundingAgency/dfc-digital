using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using System;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions.Tests
{
    public class RecycleBinProcesserTests
    {
        private readonly IWebAppContext fakeWebAppContext;
        private readonly IRecycleBinRepository fakeRecycleBinRepository;

        public RecycleBinProcesserTests()
        {
            fakeWebAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            fakeRecycleBinRepository = A.Fake<IRecycleBinRepository>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(2000)]
        public void RunProcessTest(int itemCount)
        {
            //Assin
            A.CallTo(() => fakeRecycleBinRepository.DeleteVacanciesPermanently(A<int>._)).DoesNothing();
            A.CallTo(() => fakeWebAppContext.CheckAuthenticationByAuthCookie()).DoesNothing();

            //Instantiate & Act
            var recycleBinProcesser = new RecycleBinProcesser(fakeRecycleBinRepository, fakeWebAppContext);

            //Act
           recycleBinProcesser.RunProcess(itemCount);

            //Assert
            A.CallTo(() => fakeWebAppContext.CheckAuthenticationByAuthCookie()).MustHaveHappened();
            A.CallTo(() => fakeRecycleBinRepository.DeleteVacanciesPermanently(A<int>.That.Matches(x => x.Equals(itemCount)))).MustHaveHappened();
        }
    }
}