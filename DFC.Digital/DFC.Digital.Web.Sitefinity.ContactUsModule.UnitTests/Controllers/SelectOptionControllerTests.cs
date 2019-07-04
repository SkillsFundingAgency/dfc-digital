using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
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
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public SelectOptionControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData("Why would you like to contact us?", "/contact-us/feedback/", "/contact-us/technical/", "/contact-us/contact-adviser/")]
        public void IndexGetTest(string title, string generalFeedbackPage, string technicalFeedbackPage, string contactAdviserPage)
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContactUsAutomapperProfile>();
            });

            //Assign
            var controller = new SelectOptionController(fakeApplicationLogger, mapperCfg.CreateMapper(), fakeSessionStorage)
            {
               Title = title,
               ContactAdviserPage = contactAdviserPage,
               TechnicalFeedbackPage = technicalFeedbackPage,
               GeneralFeedbackPage = generalFeedbackPage
            };

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            controllerResult.ShouldRenderDefaultView().WithModel<ContactOptionsViewModel>(
                vm =>
                {
                    vm.Title.Should().BeEquivalentTo(controller.Title);
                });
        }

        [Theory]
        [InlineData(true, ContactOption.ContactAdviser)]
        [InlineData(false, ContactOption.ContactAdviser)]
        [InlineData(true, ContactOption.Technical)]
        [InlineData(false, ContactOption.Technical)]
        [InlineData(true, ContactOption.Feedback)]
        [InlineData(false, ContactOption.Feedback)]
        [InlineData(false, null)]
        [InlineData(true, null)]
        public void SubmitTests(bool modelStateValid, ContactOption? contactOption)
        {
            //Assign
            var postModel = new ContactOptionsViewModel();

            //Setup and configure fake mapper
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContactUsAutomapperProfile>();
            });

            var controller = new SelectOptionController(fakeApplicationLogger, mapperCfg.CreateMapper(), fakeSessionStorage);

            A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();

            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(ContactOptionsViewModel.ContactOptionType), nameof(ContactOptionsViewModel.ContactOptionType));
            }

            if (contactOption != null)
            {
                postModel.ContactOptionType = contactOption.GetValueOrDefault();
            }

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
            else if (modelStateValid && contactOption == null)
            {
                controllerResult.ShouldRenderDefaultView().WithModel<ContactOptionsViewModel>();
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
