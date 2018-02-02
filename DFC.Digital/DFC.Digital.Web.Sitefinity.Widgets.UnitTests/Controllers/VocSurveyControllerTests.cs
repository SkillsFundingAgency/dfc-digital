using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.GovUkNotify.Base;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Controllers
{
    public class VocSurveyControllerTests
    {

        [Theory]
        [InlineData("test", new object[] { "jpprofile", "clientid" }, new object[] { "", "1665229681.1514888907" }, true)]
        [InlineData("testtest", new object[] { "jpprofile", "clientid" }, new object[] { "", "" }, false)]
        public void IndexSubmitEmailTest(string emailAddress, object key, object value, bool success)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var govUkNotify = A.Fake<IGovUkNotify>(ops => ops.Strict());
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());
            var emailRequest = new VocSurveyViewModel { EmailAddress = emailAddress };
            var dummynotifyResponse = success;

            // Create NotifyUK Dictionary
            var vocSurveyPersonalisationn = new VocSurveyPersonalisation();
            for (var index = 0; index < ((IList)value).Count - 1; index++)
            {
                vocSurveyPersonalisationn.Personalisation.Add(((IList)key)[index].ToString(), ((IList)value)[index].ToString());
            }

            // Set up calls
            A.CallTo(() => webAppContext.GetVocCookie(Constants.VocPersonalisationCookieName)).Returns(vocSurveyPersonalisationn);
            A.CallTo(() => govUkNotify.SubmitEmail(A<string>._, A<VocSurveyPersonalisation>._)).Returns(dummynotifyResponse);

            //Instantiate
            var vocSurveyController = new VocSurveyController(govUkNotify, webAppContext, loggerFake);

            //Act
            var indexMethodCall = vocSurveyController.WithCallTo(c => c.Index(emailRequest));

            if (!string.IsNullOrEmpty(emailAddress))
            {
                //Assert
                indexMethodCall.ShouldRenderView("Response");

                A.CallTo(() => webAppContext.GetVocCookie(Constants.VocPersonalisationCookieName)).MustHaveHappened();
                A.CallTo(() => govUkNotify.SubmitEmail(emailAddress, A<VocSurveyPersonalisation>.That.IsSameAs(vocSurveyPersonalisationn))).MustHaveHappened();
            }
            else
            {
                //Assert
                indexMethodCall.ShouldRenderDefaultView();

                A.CallTo(() => webAppContext.GetVocCookie(Constants.VocPersonalisationCookieName)).MustNotHaveHappened();
                A.CallTo(() => govUkNotify.SubmitEmail(A<string>._, A<VocSurveyPersonalisation>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData("test")]
        [InlineData("")]
        public void IndexUrlNameTest(string urlname)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var govUkNotify = A.Fake<IGovUkNotify>(ops => ops.Strict());
            var fakeWebAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());

            // Set up calls
            A.CallTo(() => govUkNotify.SubmitEmail(A<string>._, null)).Returns(false);

            //Instantiate & Act
            var vocSurveyController = new VocSurveyController(govUkNotify, fakeWebAppContext, loggerFake)
            {
                EmailSentText = nameof(VocSurveyController.EmailSentText),
                EmailNotSentText = nameof(VocSurveyController.EmailNotSentText),
                DontHaveEmailText = nameof(VocSurveyController.DontHaveEmailText),
                AgeLimitText = nameof(VocSurveyController.AgeLimitText),
                FormIntroText = nameof(VocSurveyController.FormIntroText),
            };

            // Act
            var indexMethodCall = vocSurveyController.WithCallTo(c => c.Index(urlname));

            //Assert
            indexMethodCall.ShouldRenderDefaultView();
            A.CallTo(() => govUkNotify.SubmitEmail(A<string>._, A<VocSurveyPersonalisation>._)).MustNotHaveHappened();
        }

        [Fact]
        public void IndexTest()
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var govUkNotify = A.Fake<IGovUkNotify>(ops => ops.Strict());
            var webAppContext = A.Fake<IWebAppContext>(ops => ops.Strict());

            //Instantiate & Act
            var vocSurveyController = new VocSurveyController(govUkNotify, webAppContext, loggerFake);

            //Act
            var indexMethodCall = vocSurveyController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall.ShouldRenderDefaultView();
            A.CallTo(() => govUkNotify.SubmitEmail(A<string>._, A<VocSurveyPersonalisation>._)).MustNotHaveHappened();
        }

        [Theory]
        [InlineData("user1@user.com", new object[] { "jpprofile", "clientid" }, new object[] { "children's-nurse", "1665229681.1514888907" }, true)]
        [InlineData("user2@user.com", new object[] { "jpprofile", "clientid" }, new object[] { "Unknown", "Unknown" }, true)]
        [InlineData("user2@user.com", new object[] { "jpprofile", "clientid" }, new object[] { "", "" }, false)]
        public void SendEmailTest(string emailAddress, object key, object value, bool returnVal)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var govUkNotifyFake = A.Fake<IGovUkNotify>(ops => ops.Strict());

            // Create NotifyUK Dictionary
            var vocSurveyPersonalisationn = new VocSurveyPersonalisation { Personalisation = new Dictionary<string, string>() };
            for (var index = 0; index < ((IList)value).Count - 1; index++)
            {
                vocSurveyPersonalisationn.Personalisation.Add(((IList)key)[index].ToString(), ((IList)value)[index].ToString());
            }

            // Set up calls
            A.CallTo(() => webAppContextFake.GetVocCookie(Constants.VocPersonalisationCookieName)).Returns(vocSurveyPersonalisationn);
            A.CallTo(() => govUkNotifyFake.SubmitEmail(emailAddress, vocSurveyPersonalisationn)).Returns(returnVal);

            //Instantiate & Act
            var vocSurveyController = new VocSurveyController(govUkNotifyFake, webAppContextFake, loggerFake);

            //Act
            var sendEmailMethodCall = vocSurveyController.WithCallTo(c => c.SendEmail(emailAddress));

            //Assert
            sendEmailMethodCall.ShouldReturnJson().ShouldBeEquivalentTo(new JsonResult { Data = returnVal });
            A.CallTo(() => webAppContextFake.GetVocCookie(Constants.VocPersonalisationCookieName)).MustHaveHappened();
            A.CallTo(() => govUkNotifyFake.SubmitEmail(emailAddress, vocSurveyPersonalisationn)).MustHaveHappened();
        }
    }
}