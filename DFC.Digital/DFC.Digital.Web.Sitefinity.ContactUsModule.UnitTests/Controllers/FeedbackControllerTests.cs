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

    public class FeedbackControllerTests
    {
        #region Private Fields

        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly ISitefinityCurrentContext fakeSitefinityCurrentContext;
        private readonly IMapper fakeMapper;
        private readonly IWebAppContext fakeWebAppcontext;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public FeedbackControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>();
            fakeSitefinityCurrentContext = A.Fake<ISitefinityCurrentContext>();
            fakeWebAppcontext = A.Fake<IWebAppContext>();
            fakeMapper = A.Fake<IMapper>();
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexGetTest(bool validSessionVm)
        {
            var controller = new FeedbackController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage)
            {
                Title = nameof(FeedbackController.Title),
                PersonalInformation = nameof(FeedbackController.PersonalInformation),
                NextPageUrl = nameof(FeedbackController.NextPageUrl)
            };

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            if (!validSessionVm)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());
            }

            //Assert
            if (validSessionVm)
            {
                controllerResult.ShouldRenderDefaultView()
                .WithModel<GeneralFeedbackViewModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(controller.Title);
                    vm.Hint.Should().BeEquivalentTo(controller.PersonalInformation);
                    vm.NextPageUrl.Should().BeEquivalentTo(controller.NextPageUrl);
                });

                A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
            }
            else
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPageUrl);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SubmitTests(bool modelStateValid)
        {
            //Assign
            var postModel = new GeneralFeedbackViewModel();
            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());

            var controller = new FeedbackController(fakeEmailTemplateRepository, fakeSitefinityCurrentContext, fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage);


            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(GeneralFeedbackViewModel.FeedbackQuestionType), nameof(GeneralFeedbackViewModel.Feedback));
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            //Assert
            if (modelStateValid)
            {
                controllerResult.ShouldRedirectTo(controller.NextPageUrl);
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<GeneralFeedbackViewModel>()
                    .AndModelError(nameof(GeneralFeedbackViewModel.Title));
            }
        }

        #endregion Action Tests
    }
}
