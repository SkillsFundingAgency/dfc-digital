using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Controllers
{
    public class TechnicalControllerTests
    {
        #region Private Fields
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IMapper fakeMapper;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        private readonly IWebAppContext fakeWebAppContext;
        #endregion Private Fields

        #region Constructors

        public TechnicalControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeMapper = A.Fake<IMapper>(ops => ops.Strict());
            fakeWebAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
        }

        #endregion Constructors
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]

        public void IndexGetTest(bool isContentAuthoringSite, bool hasValidSession)
        {
            //Set up
            A.CallTo(() => fakeWebAppContext.IsContentAuthoringSite).Returns(isContentAuthoringSite);

            if (hasValidSession)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs() { ContactUsOption = new ContactUsOption() });
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }

            var technicalController = new TechnicalController(fakeApplicationLogger, fakeMapper, fakeWebAppContext, fakeSessionStorage);

            //Act
            var indexMethodCallResult = technicalController.WithCallTo(c => c.Index());

            //Assert
            if (!isContentAuthoringSite && !hasValidSession)
            {
                indexMethodCallResult.ShouldRedirectTo(technicalController.ContactOptionPageUrl);
            }
            else
            {
                indexMethodCallResult.ShouldRenderDefaultView()
                   .WithModel<TechnicalFeedbackViewModel>(vm =>
                   {
                       vm.CharacterLimit.Should().Be(technicalController.CharacterLimit);
                       vm.MessageLabel.Should().Be(technicalController.MessageLabel);
                       vm.PageIntroduction.Should().Be(technicalController.PageIntroduction);
                       vm.PersonalInformation.Should().Be(technicalController.PersonalInformation);
                       vm.Title.Should().Be(technicalController.Title);
                       vm.NextPageUrl.Should().Be(technicalController.NextPageUrl);
                   });
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SubmitTests(bool validSubmission)
        {
            //Set up
            var technicalController = new TechnicalController(fakeApplicationLogger, fakeMapper, fakeWebAppContext,  fakeSessionStorage);
            var technicalFeedbackViewModel = new TechnicalFeedbackViewModel() { Message = "Dummy message" };
            var dummyErrorKey = "dummyErrorKey";

            //if its not valid fake an error
            if (!validSubmission)
            {
                technicalController.ModelState.AddModelError(dummyErrorKey, "dummyErrorMessage");
            }

            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());
            A.CallTo(() => fakeMapper.Map(A<TechnicalFeedbackViewModel>._, A<ContactUsViewModel>._)).Returns(new ContactUsViewModel());
            A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();

            //Act
            var indexMethodCallResult = technicalController.WithCallTo(c => c.Index(technicalFeedbackViewModel));

            //Assert
            if (!validSubmission)
            {
                indexMethodCallResult.ShouldRenderDefaultView()
                    .WithModel<TechnicalFeedbackViewModel>(vm =>
                    {
                        vm.CharacterLimit.Should().Be(technicalController.CharacterLimit);
                        vm.MessageLabel.Should().Be(technicalController.MessageLabel);
                        vm.PageIntroduction.Should().Be(technicalController.PageIntroduction);
                        vm.PersonalInformation.Should().Be(technicalController.PersonalInformation);
                        vm.Title.Should().Be(technicalController.Title);
                        vm.NextPageUrl.Should().Be(technicalController.NextPageUrl);
                    }).AndModelError(dummyErrorKey);
            }
            else
            {
                indexMethodCallResult.ShouldRedirectTo(technicalController.NextPageUrl);
            }
        }
    }
}