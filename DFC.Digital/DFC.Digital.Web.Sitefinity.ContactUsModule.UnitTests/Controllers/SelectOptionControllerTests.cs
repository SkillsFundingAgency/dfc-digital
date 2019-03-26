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

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class SelectOptionControllerTests
    {
        #region Private Fields
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly ISitefinityCurrentContext fakeSitefinityCurrentContext;
        private readonly IMapper fakeMapper;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public SelectOptionControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>();
            fakeSitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>();
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(ContactOption.ContactAdviser)]
        [InlineData(ContactOption.Feedback)]
        [InlineData(ContactOption.Technical)]
        public void IndexGetTest(ContactOption contactOption)
        {
            //Assign
            var controller = new SelectOptionController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeMapper, fakeSessionStorage);

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            controllerResult.ShouldRenderDefaultView()
                .WithModel<ContactOptionsViewModel>(vm =>
                {
                    vm.ContactOptionType.Should().Be(contactOption);
                });
            A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
        }

        [Theory]
        [InlineData(true, true, ContactOption.ContactAdviser)]
        [InlineData(true, true, ContactOption.Technical)]
        [InlineData(true, true, ContactOption.Feedback)]
        public void SubmitTests(bool modelStateValid, bool validSubmission, ContactOption contactOption)
        {
            //Assign
            var postModel = modelStateValid
                ? new ContactOptionsViewModel
                {
                    ContactOptionType = contactOption
                }
                : new ContactOptionsViewModel();

            var controller = new SelectOptionController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeMapper, fakeSessionStorage);

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            //Assert
            if (modelStateValid && contactOption == ContactOption.ContactAdviser)
            {
                controllerResult.ShouldRedirectTo(controller.ContactAdviserPage);
            }
            else if (modelStateValid && contactOption == ContactOption.Technical)
            {
                controllerResult.ShouldRedirectTo(controller.TechnicalFeedbackPage);
            }
            else if (modelStateValid && contactOption == ContactOption.Feedback)
            {
                controllerResult.ShouldRedirectTo(controller.GeneralFeedbackPage);
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<ContactOptionsViewModel>()
                    .AndModelError(nameof(ContactOptionsViewModel.ContactOptionType));
            }
        }

        #endregion Action Tests

        #region Private Methods

        #endregion Private Methods
    }
}
