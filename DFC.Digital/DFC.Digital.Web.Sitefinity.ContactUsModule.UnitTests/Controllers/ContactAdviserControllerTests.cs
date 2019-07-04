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
    public class ContactAdviserControllerTests
    {
        #region Private Fields

        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IMapper fakeMapper;
        private readonly IWebAppContext fakeWebAppcontext;
        private readonly ISessionStorage<ContactUs> fakeSessionStorage;
        #endregion Private Fields

        #region Constructors

        public ContactAdviserControllerTests()
        {
            fakeSessionStorage = A.Fake<ISessionStorage<ContactUs>>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeWebAppcontext = A.Fake<IWebAppContext>();
            fakeMapper = A.Fake<IMapper>();
        }

        #endregion Constructors

        #region Action Tests

        [Theory]
        [InlineData(true, "Why would you like to contact us?", "/contact-us/feedback/", "/contact-us/technical/", "/contact-us/contact-adviser/", "You can enter up to 1000 characters")]
        [InlineData(false, "Why would you like to contact us?", "/contact-us/feedback/", "/contact-us/technical/", "/contact-us/contact-adviser/", "You can enter up to 1000 characters")]
        public void IndexSetDefaultsTest(bool validSessionVm, string title, string personalInformation, string nextPageUrl, string contactOptionPageUrl, string characterLimit)
        {
            //Assign
            var controller = new ContactAdviserController(fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage)
            {
                Title = title,
                PersonalInformation = personalInformation,
                NextPage = nextPageUrl,
                ContactOptionPage = contactOptionPageUrl,
                CharacterLimit = characterLimit
            };

            if (!validSessionVm)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs { ContactUsOption = new ContactUsOption() { ContactOptionType = ContactOption.ContactAdviser } });
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            if (validSessionVm)
            {
                controllerResult.ShouldRenderDefaultView().WithModel<ContactAdviserViewModel>(
                    vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(controller.Title);
                        vm.NextPage.Should().BeEquivalentTo(controller.NextPage);
                        vm.Hint.Should().BeEquivalentTo(controller.PersonalInformation);
                    });
            }
            else
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPage);
            }
        }

        [Theory]
        [InlineData(true, ContactOption.ContactAdviser, false)]
        [InlineData(false, ContactOption.ContactAdviser, true)]
        [InlineData(true, ContactOption.Technical, true)]
        [InlineData(true, null, true)]
        public void IndexGetTest(bool validSessionVm, ContactOption? contactOption, bool expectToBeRedirected)
        {
            var controller = new ContactAdviserController(fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage)
            {
                Title = nameof(ContactAdviserController.Title),
                PersonalInformation = nameof(ContactAdviserController.PersonalInformation),
                NextPage = nameof(ContactAdviserController.NextPage)
            };

            //Setup fakes
            if (!validSessionVm)
            {
                A.CallTo(() => fakeSessionStorage.Get()).Returns(null);
            }
            else
            {
                if (contactOption is null)
                {
                    A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs() { ContactUsOption = new ContactUsOption() });
                }
                else
                {
                    A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs() { ContactUsOption = new ContactUsOption() { ContactOptionType = (ContactOption)contactOption } });
                }
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index());

            //Assert
            if (expectToBeRedirected)
            {
                controllerResult.ShouldRedirectTo(controller.ContactOptionPage);
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<ContactAdviserViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(controller.Title);
                        vm.Hint.Should().BeEquivalentTo(controller.PersonalInformation);
                    });
            }

            A.CallTo(() => fakeSessionStorage.Get()).MustHaveHappened(1, Times.Exactly);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SubmitTests(bool modelStateValid)
        {
            //Assign
            var postModel = new ContactAdviserViewModel();
            A.CallTo(() => fakeSessionStorage.Get()).Returns(new ContactUs());

            var controller = new ContactAdviserController(fakeApplicationLogger, fakeMapper, fakeWebAppcontext, fakeSessionStorage)
            {
                Title = nameof(ContactAdviserController.Title)
            };

            if (!modelStateValid)
            {
                controller.ModelState.AddModelError(nameof(ContactAdviserViewModel.ContactAdviserQuestionType), nameof(ContactAdviserViewModel.Message));
            }
            else
            {
                A.CallTo(() => fakeSessionStorage.Save(A<ContactUs>._)).DoesNothing();
            }

            //Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(postModel));

            //Assert
            if (modelStateValid)
            {
                controllerResult.ShouldRedirectTo(controller.NextPage);
            }
            else
            {
                controllerResult.ShouldRenderDefaultView()
                    .WithModel<ContactAdviserViewModel>()
                    .AndModelError(nameof(ContactAdviserViewModel.ContactAdviserQuestionType));
            }
        }

        #endregion Action Tests
    }
}
