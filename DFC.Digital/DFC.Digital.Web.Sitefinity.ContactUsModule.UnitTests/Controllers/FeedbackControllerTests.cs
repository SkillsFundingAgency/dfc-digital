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
    public class FeedbackControllerTests
    {
        #region Private Fields

        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IApplicationLogger fakeApplicationLogger;
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
            fakeWebAppcontext = A.Fake<IWebAppContext>();
            fakeMapper = A.Fake<IMapper>();
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(true, "Character limit is 1000.", "/contact-us/select-option/", "/contact-us/your-details/", "continue")]
        [InlineData(false, "Character limit is 1000.", "/contact-us/select-option/", "/contact-us/your-details/", "continue")]

        public void IndexGetTest(bool validSessionVm, string characterLimit, string contactOptionPageUrl, string nextPageUrl, string continueText)
        {
            var controller = new FeedbackController(fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage)
            {
                Title = nameof(FeedbackController.Title),
                PersonalInformation = nameof(FeedbackController.PersonalInformation),
                NextPageUrl = nextPageUrl,
                CharacterLimit = characterLimit,
                ContactOptionPage = contactOptionPageUrl,
                ContinueText = continueText
            };

            if (!validSessionVm)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();
                A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs { ContactUsOption = new ContactUsOption() });
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            if (validSessionVm)
            {
                controllerResult.ShouldRenderDefaultView()
                .WithModel<GeneralFeedbackViewModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(controller.Title);
                    vm.PersonalInformation.Should().BeEquivalentTo(controller.PersonalInformation);
                    vm.NextPage.Should().BeEquivalentTo(controller.NextPageUrl);
                });

                A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
            }
            else
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPage);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SubmitTests(bool modelStateValid)
        {
            //Assign
            var postModel = new GeneralFeedbackViewModel();

            A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();

            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs { ContactUsOption = new ContactUsOption() });

            var controller = new FeedbackController(fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage);

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
                    .AndModelError(nameof(GeneralFeedbackViewModel.FeedbackQuestionType));
            }
        }

        #endregion Action Tests
    }
}
