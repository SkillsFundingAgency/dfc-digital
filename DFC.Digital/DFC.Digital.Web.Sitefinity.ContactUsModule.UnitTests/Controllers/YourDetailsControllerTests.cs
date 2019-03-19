/*using AutoMapper;
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
        private readonly ISessionStorage<ContactUsViewModel> fakeSessionStorage;

        #endregion Private Fields

        #region Constructors

        public YourDetailsControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUsViewModel>>(ops => ops.Strict());
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
            //Assign
            A.CallTo(() => fakeSessionStorage.Get())
                .Returns(validSessionVm
                ? new ContactUsViewModel { ContactOption = contactOption }
                : null);
            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeMapper, fakeSessionStorage)
            {
                PageTitle = nameof(YourDetailsController.PageTitle),
                PageIntroductionTwo = nameof(YourDetailsController.PageIntroductionTwo),
                PageIntroduction = nameof(YourDetailsController.PageIntroduction)
            };

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(contactOption));

            //Assert
            controllerResult.ShouldRenderDefaultView()
                .WithModel<ContactUsViewModel>(vm =>
                {
                    vm.ContactOption.Should().Be(contactOption);
                });
            A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void SubmitTests(bool modelStateValid, bool validSubmission)
        {
            //Assign
            var postModel = modelStateValid
                ? new ContactUsViewModel
                {
                    FirstName = nameof(ContactUsViewModel.FirstName),
                    LastName = nameof(ContactUsViewModel.LastName),
                    Email = "test@mail.com",
                    EmailConfirm = "test@mail.com",
                    DobDay = "10",
                    DobMonth = "10",
                    DobYear = "1970",
                    TermsAndConditions = true
                }
                : new ContactUsViewModel();
            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);

            var controller = new YourDetailsController(fakeApplicationLogger, fakeSendEmailService, fakeAsyncHelper, fakeMapper, fakeSessionStorage)
            {
                SuccessMessage = nameof(YourDetailsController.SuccessMessage),
                FailureMessage = nameof(YourDetailsController.FailureMessage)
            };

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            //Assert
            if (modelStateValid)
            {
                controllerResult.ShouldRenderView("ThankYou")
                    .WithModel<ContactUsResultViewModel>(vm =>
                    {
                        vm.Message.Should().Be(validSubmission ? controller.SuccessMessage : controller.FailureMessage);
                    });
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<ContactUsViewModel>()
                    .AndModelError(nameof(ContactUsViewModel.DateOfBirth));
            }
        }

        #endregion Action Tests
    }
}*/