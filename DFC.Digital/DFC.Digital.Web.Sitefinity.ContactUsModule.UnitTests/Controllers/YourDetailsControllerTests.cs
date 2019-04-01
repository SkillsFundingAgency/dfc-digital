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
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        private readonly IWebAppContext fakeContext;

        #endregion Private Fields

        #region Constructors

        public YourDetailsControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeSendEmailService = A.Fake<INoncitizenEmailService<ContactUsRequest>>(ops => ops.Strict());
            fakeContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            A.CallTo(() => fakeContext.IsContentAuthoringSite).Returns(false);
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(ContactOption.ContactAdviser, true)]
        [InlineData(ContactOption.Feedback, false)]
        [InlineData(ContactOption.Technical, true)]
        public void IndexGetTest(ContactOption contactOption, bool validSessionVm)
        {
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeContext, fakeSessionStorage)
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
            if (!validSessionVm)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs { ContactUsOption = new ContactUsOption { ContactOptionType = contactOption }, ContactAnAdviserFeedback = new ContactAnAdviserFeedback(), GeneralFeedback = new GeneralFeedback(), TechnicalFeedback = new TechnicalFeedback() });
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            if (validSessionVm)
            {
                AssertIndexGetViews(contactOption, controllerResult, controller);
            }
            else
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPageUrl);
            }
        }

        [Theory]
        [InlineData(ContactOption.Feedback, ContactOption.Feedback, false)]
        [InlineData(ContactOption.Feedback, ContactOption.ContactAdviser, true)]
        [InlineData(ContactOption.ContactAdviser, ContactOption.Feedback, true)]
        [InlineData(ContactOption.ContactAdviser, ContactOption.ContactAdviser, false)]
        [InlineData(ContactOption.Technical, ContactOption.Feedback, true)]
        [InlineData(ContactOption.Feedback, ContactOption.Technical, true)]
        [InlineData(ContactOption.Technical, ContactOption.Technical, false)]
        public void IndexGetSessionTests(ContactOption contactOption, ContactOption contactOptionProperty, bool redirectExpected)
        {
            // Assign
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeContext, fakeSessionStorage)
            {
                ContactOption = contactOptionProperty,
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

            var sessionObject = GetSessionObject(contactOption);

            A.CallTo(() => fakeSessionStorage.Get()).Returns(sessionObject);

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            // Assert
            if (redirectExpected)
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPageUrl);
            }
            else
            {
                AssertIndexGetViews(contactOption, controllerResult, controller);
            }
        }

        [Theory]
        [InlineData(true, true, true, ContactOption.Feedback)]
        [InlineData(true, false, false, ContactOption.Feedback)]
        [InlineData(false, false, false, ContactOption.Technical)]
        [InlineData(true, true, true, ContactOption.Technical)]
        public void SubmitTests(bool modelStateValid, bool validSubmission, bool validSession, ContactOption contactOption)
        {
            //Assign
            var postModel = new ContactUsWithConsentViewModel();
            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);
            A.CallTo(() => fakeSessionStorage.Get()).Returns(validSession ? GetSessionObject(contactOption) : null);
            A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();

            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeContext, fakeSessionStorage)
            {
                SuccessPageUrl = nameof(YourDetailsController.SuccessPageUrl),
                FailurePageUrl = nameof(YourDetailsController.FailurePageUrl),
                ContactOption = contactOption
            };

            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(ContactUsWithDobPostcodeViewModel.Firstname), nameof(ContactUsWithDobPostcodeViewModel.Firstname));
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Submit(postModel));

            //Assert
            if (modelStateValid)
            {
                if (validSession)
                {
                    A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(2, Times.Exactly);

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
                    A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);

                    controllerResult.ShouldRedirectTo(controller.ContactOptionPageUrl);
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
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        [InlineData(true, true, false)]
        public void SubmitDetailsTests(bool modelStateValid, bool validSubmission, bool validSession)
        {
            //Assign
            var postModel = new ContactUsWithDobPostcodeViewModel();
            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);
            A.CallTo(() => fakeSessionStorage.Get()).Returns(validSession ? GetSessionObject(ContactOption.ContactAdviser) : null);
            A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();

            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeContext, fakeSessionStorage)
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
                if (validSession)
                {
                    A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(2, Times.Exactly);

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
                    A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);

                    controllerResult.ShouldRedirectTo(controller.ContactOptionPageUrl);
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

        #region Private Methods
        private static ContactUs GetSessionObject(ContactOption contactOption)
        {
            var sessionObject = new ContactUs
            {
                ContactUsOption = new ContactUsOption { ContactOptionType = contactOption },
                ContactAnAdviserFeedback =
                    contactOption == ContactOption.ContactAdviser ? new ContactAnAdviserFeedback { Message = nameof(ContactAnAdviserFeedback.Message) } : null,
                GeneralFeedback = contactOption == ContactOption.Feedback ? new GeneralFeedback { Feedback = nameof(GeneralFeedback.Feedback) } : null,
                TechnicalFeedback = contactOption == ContactOption.Technical ? new TechnicalFeedback { Message = nameof(TechnicalFeedback.Message) } : null
            };
            return sessionObject;
        }

        private static void AssertIndexGetViews(ContactOption contactOption, ControllerResultTest<YourDetailsController> controllerResult, YourDetailsController controller)
        {
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
                        vm.DoYouWantUsToContactUsText.Should().BeEquivalentTo(controller.DoYouWantUsToContactUsText);
                        vm.PageIntroduction.Should().BeEquivalentTo(controller.NonAdviserIntroduction);
                        vm.TermsAndConditionsText.Should().BeEquivalentTo(controller.TermsAndConditionsText);
                    });
            }
        }

        #endregion

    }
}