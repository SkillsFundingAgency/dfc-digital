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
        [InlineData(5, false)]
        [InlineData(2000, true)]
        public void RunProcessTest(int itemCount, bool validAuthCookie)
        {
            //Assin
            A.CallTo(() => fakeRecycleBinRepository.DeleteVacanciesPermanently(A<int>._)).Returns(true);
            if (validAuthCookie)
            {
                A.CallTo(() => fakeWebAppContext.CheckAuthenticationByAuthCookie()).DoesNothing();
            }
            else
            {
                A.CallTo(() => fakeWebAppContext.CheckAuthenticationByAuthCookie())
                    .Throws(new UnauthorizedAccessException());
            }

            try
            {
                //Instantiate & Act
                var recycleBinProcesser = new RecycleBinProcesser(fakeRecycleBinRepository, fakeWebAppContext);

                //Act
                recycleBinProcesser.RunProcess(itemCount);
            }
            catch (UnauthorizedAccessException)
            {
            }

            //Assert
            A.CallTo(() => fakeWebAppContext.CheckAuthenticationByAuthCookie()).MustHaveHappened();
            if (validAuthCookie)
            {
                A.CallTo(() => fakeRecycleBinRepository.DeleteVacanciesPermanently(A<int>.That.Matches(x => x.Equals(itemCount)))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeRecycleBinRepository.DeleteVacanciesPermanently(A<int>._)).MustNotHaveHappened();
            }
        }
    }
}