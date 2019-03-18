namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Controllers
{
    using AutoMapper;
    using DFC.Digital.Core;
    using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers;
    using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
    using FakeItEasy;
    using FluentAssertions;
    using TestStack.FluentMVCTesting;
    using Xunit;

    public class TechnicalControllerTests
    {
        #region Private Fields
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IMapper fakeMapper;
        private readonly ISessionStorage<ContactUsViewModel> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public TechnicalControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUsViewModel>>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeMapper = A.Fake<IMapper>(ops => ops.Strict());
        }

        #endregion Constructors
        [Fact]
        public void IndexGetTest()
        {
            //Set up
            var technicalController = new TechnicalController(fakeApplicationLogger, fakeMapper, fakeSessionStorage);

            //Act
            var indexMethodCallResult = technicalController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCallResult.ShouldRenderDefaultView()
               .WithModel<ContactUsTechnicalViewModel>(vm =>
               {
                   vm.ContactOption.Should().Be(ContactOption.Technical);
                   vm.CharacterLimit.Should().Be(technicalController.CharacterLimit);
                   vm.MessageLabel.Should().Be(technicalController.MessageLabel);
                   vm.PageIntroduction.Should().Be(technicalController.PageIntroduction);
                   vm.PersonalInformation.Should().Be(technicalController.PersonalInformation);
                   vm.Title.Should().Be(technicalController.Title);

               });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SubmitTests(bool validSubmission)
        {
            //Set up
            var technicalController = new TechnicalController(fakeApplicationLogger, fakeMapper, fakeSessionStorage);
            var contactUsTechnicalViewModel = new ContactUsTechnicalViewModel() { TechnicalFeedback = new Data.Model.TechnicalFeedback() { Message = "Dummy message" } };
            var dummyErrorKey = "dummyErrorKey";

            //if its not valid fake an error
            if (!validSubmission)
            {
                technicalController.ModelState.AddModelError(dummyErrorKey, "dummyErrorMessage");
            }

            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUsViewModel());
            A.CallTo(() => fakeMapper.Map(A<ContactUsTechnicalViewModel>._, A<ContactUsViewModel>._)).Returns(new ContactUsViewModel());
            A.CallTo(() => fakeSessionStorage.Save(A<ContactUsViewModel>._)).DoesNothing();

            //Act
            var indexMethodCallResult = technicalController.WithCallTo(c => c.Index(contactUsTechnicalViewModel));

            //Assert
            if (!validSubmission)
            {
                indexMethodCallResult.ShouldRenderDefaultView()
                    .WithModel<ContactUsTechnicalViewModel>(vm =>
                    {
                        vm.CharacterLimit.Should().Be(technicalController.CharacterLimit);
                        vm.MessageLabel.Should().Be(technicalController.MessageLabel);
                        vm.PageIntroduction.Should().Be(technicalController.PageIntroduction);
                        vm.PersonalInformation.Should().Be(technicalController.PersonalInformation);
                        vm.Title.Should().Be(technicalController.Title);
                    }).AndModelError(dummyErrorKey);
            }
            else
            {
                indexMethodCallResult.ShouldRedirectTo(technicalController.NextPageUrl);
            }
        }
    }
}