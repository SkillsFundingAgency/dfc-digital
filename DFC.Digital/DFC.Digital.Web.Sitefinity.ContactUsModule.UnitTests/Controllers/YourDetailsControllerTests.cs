using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class YourDetailsControllerTests
    {
        #region Private Fields

        private readonly INoncitizenEmailService<ContactUsRequest> fakeSendEmailService;
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IMapper fakeMapper;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;

        #endregion Private Fields

        #region Constructors

        public YourDetailsControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeSendEmailService = A.Fake<INoncitizenEmailService<ContactUsRequest>>(ops => ops.Strict());
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(ContactOption.ContactAdviser, true)]
        [InlineData(ContactOption.Feedback, false)]
        [InlineData(ContactOption.Technical, true)]
        public void IndexGetTest(ContactOption contactOption, bool validSessionVm)
        {
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeMapper, fakeSessionStorage)
            {
                ContactOption = contactOption,
                PageTitle = nameof(YourDetailsController.PageTitle),
                AdviserIntroductionTwo = nameof(YourDetailsController.AdviserIntroductionTwo),
                AdviserIntroduction = nameof(YourDetailsController.AdviserIntroduction),
                NonAdviserIntroduction = nameof(YourDetailsController.NonAdviserIntroduction),
                DateOfBirthHint = nameof(YourDetailsController.DateOfBirthHint),
                PostcodeHint = nameof(YourDetailsController.PostcodeHint),
                SuccessPageUrl = nameof(YourDetailsController.SuccessPageUrl),
                DoYouWantUsToContactUsText = nameof(YourDetailsController.DoYouWantUsToContactUsText),
                TermsAndConditionsText = nameof(YourDetailsController.TermsAndConditionsText),
                TemplateName = nameof(YourDetailsController.TemplateName)
            };

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            if (contactOption == ContactOption.ContactAdviser)
            {
                controllerResult.ShouldRenderView("ContactAdvisor")
                    .WithModel<ContactUsWithDobPostcodeViewModel>(vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.PageIntroduction.Should().BeEquivalentTo(controller.AdviserIntroduction);
                        vm.PageIntroductionTwo.Should().BeEquivalentTo(controller.AdviserIntroductionTwo);
                        vm.PostcodeHint.Should().BeEquivalentTo(controller.PostcodeHint);
                        vm.DateOfBirthHint.Should().BeEquivalentTo(controller.DateOfBirthHint);
                        vm.TermsAndConditionsText.Should().BeEquivalentTo(controller.TermsAndConditionsText);
                    });
            }
            else
            {
                controllerResult.ShouldRenderView("Feedback")
                    .WithModel<ContactUsWithConsentViewModel>(vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.PageIntroduction.Should().BeEquivalentTo(controller.NonAdviserIntroduction);
                        vm.TermsAndConditionsText.Should().BeEquivalentTo(controller.TermsAndConditionsText);
                    });
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void SubmitTests(bool modelStateValid, bool validSubmission)
        {
            //Assign
            var postModel = new ContactUsWithConsentViewModel();

            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);
            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeMapper, fakeSessionStorage)
            {
                SuccessPageUrl = nameof(YourDetailsController.SuccessPageUrl),
                FailurePageUrl = nameof(YourDetailsController.FailurePageUrl)
            };

            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(ContactUsWithConsentViewModel.Firstname), nameof(ContactUsWithDobPostcodeViewModel.Firstname));
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Submit(postModel));

            //Assert
            if (modelStateValid)
            {
                A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
                if (validSubmission)
                {
                    controllerResult.ShouldRedirectTo(controller.SuccessPageUrl);
                }
                else
                {
                    controllerResult.ShouldRedirectTo(controller.FailurePageUrl);
                }
            }
            else
            {
                controllerResult.ShouldRenderView("Feedback")
                    .WithModel<ContactUsWithConsentViewModel>()
                    .AndModelErrorFor(model => model.Firstname);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void SubmitDetailsTests(bool modelStateValid, bool validSubmission)
        {
            //Assign
            var postModel = new ContactUsWithDobPostcodeViewModel();
            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);
            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeMapper, fakeSessionStorage)
            {
                SuccessPageUrl = nameof(YourDetailsController.SuccessPageUrl),
                FailurePageUrl = nameof(YourDetailsController.FailurePageUrl)
            };

            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(ContactUsWithDobPostcodeViewModel.Firstname), nameof(ContactUsWithDobPostcodeViewModel.Firstname));
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.SubmitDetails(postModel));

            //Assert
            if (modelStateValid)
            {
                A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
                if (validSubmission)
                {
                    controllerResult.ShouldRedirectTo(controller.SuccessPageUrl);
                }
                else
                {
                    controllerResult.ShouldRedirectTo(controller.FailurePageUrl);
                }
            }
            else
            {
                controllerResult.ShouldRenderView("ContactAdvisor")
                    .WithModel<ContactUsWithDobPostcodeViewModel>()
                    .AndModelErrorFor(model => model.Firstname);
            }
        }

        #endregion Action Tests
    }
}