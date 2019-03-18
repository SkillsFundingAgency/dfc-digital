using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Controllers
{
    public class ContactAdviserControllerTests
    {
        #region Private Fields
        private readonly INoncitizenEmailService<ContactUsRequest> fakeSendEmailService;
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly ISitefinityCurrentContext fakeSitefinityCurrentContext;
        private readonly IMapper fakeMapper;
        private readonly ISessionStorage<ContactUsViewModel> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public ContactAdviserControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUsViewModel>>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>();
            fakeSitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>();
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
                ? new ContactUsContactAdviserViewModel { ContactOptionType = contactOption }
                : null);
            var controller = new ContactAdviserController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeSessionStorage);

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            controllerResult.ShouldRenderDefaultView()
                .WithModel<ContactUsViewModel>(vm =>
                {
                    vm.ContactAdviser.ContactOptionType.Should().Be(contactOption);
                });
            A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void SubmitTests(bool modelStateValid, bool validSubmission, string contactOptionSelected)
        {
            //Assign
            var postModel = modelStateValid
                ? new ContactUsContactAdviserViewModel
                {
                    ContactOptionType = nameof(ContactUsContactAdviserViewModel.ContactOptionType)
                }
                : new ContactUsViewModel();
            A.CallTo(() => fakeSendEmailService.SendEmailAsync(A<ContactUsRequest>._)).Returns(validSubmission);

            var controller = new ContactAdviserController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeSessionStorage);

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            //Assert
            if (modelStateValid && contactOptionSelected == "ContactAdviser")
            {
                controllerResult.ShouldRenderView("ContactAdviser")
                    .WithModel<ContactUsContactAdviserViewModel>(vm =>
                    {
                        vm.ContactAdviser.Should().Be(validSubmission);
                    });
            }
            else if (modelStateValid && contactOptionSelected == "Technical")
            {
                controllerResult.ShouldRenderView("Technical")
                    .WithModel<ContactUsTechnicalViewModel>(vm =>
                    {
                        vm.ContactAdviser.Should().Be(validSubmission);
                    });
            }
            else if (modelStateValid && contactOptionSelected == "Feedback")
            {
                controllerResult.ShouldRenderView("Feedback")
                    .WithModel<ContactUsFeedbackViewModel>(vm =>
                    {
                        vm.ContactAdviser.Should().Be(validSubmission);
                    });
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<ContactUsViewModel>()
                    .AndModelError(nameof(ContactUsViewModel.ContactAdviser));
            }
        }

        #endregion Action Tests
    }
}
