//using Xunit;
//using DFC.Digital.Web.Sitefinity.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FakeItEasy;
//using DFC.Digital.Data.Interfaces; using DFC.Digital.Core;
//using Telerik.Sitefinity.Pages.Model;
//using FluentAssertions;
//using Telerik.Sitefinity.Modules.Pages;
//using Telerik.Sitefinity.Web;

//namespace DFC.Digital.Web.Sitefinity.Core.Tests
//{
//    public class SitefinityPageTests
//    {
//        //[Fact()]
//        public void GetContextPagePreviewTest()
//        {
//            //Fakes and dummies
//            var expectedResult = A.Dummy<PageDraft>();
//            var fakeContext = A.Fake<IWebAppContext>();
//            var fakePageManager = A.Fake<PageManager>();
//            var fakePageData = A.Fake<PageData>();
//            var fakePageSiteNode = A.Fake<PageSiteNode>();
//            var dummyCurrentPageId = Guid.NewGuid();
//            var fakeSfContext = A.Fake<ISitefinityCurrentContext>();

//            A.CallTo(() => fakeSfContext.CurrentPageManager).Returns(fakePageManager);
//            A.CallTo(() => fakeSfContext.CurrentPage).Returns(fakePageData);
//            A.CallTo(() => fakeSfContext.CurrentNode).Returns(fakePageSiteNode);

//            A.CallTo(() => fakePageSiteNode.PageId).Returns(dummyCurrentPageId);
//            A.CallTo(() => fakePageManager.GetPageData(A<Guid>.That.IsEqualTo(dummyCurrentPageId))).Returns(fakePageData);
//            A.CallTo(() => fakePageData.Id).Returns(dummyCurrentPageId);
//            A.CallTo(() => fakePageManager.GetPreview(A<Guid>.That.IsEqualTo(dummyCurrentPageId))).Returns(expectedResult);

//            //Instantiate
//            var sitefinityPage = new SitefinityPage(fakeContext, fakeSfContext);

//            //Act
//            var result = sitefinityPage.GetContextPagePreview();

//            //Assert
//            expectedResult.Should().BeEquivalentTo(result);
//            A.CallTo(() => fakePageSiteNode.PageId).MustHaveHappened();
//            A.CallTo(() => fakePageManager.GetPageData(A<Guid>.That.IsEqualTo(dummyCurrentPageId))).MustHaveHappened();
//            A.CallTo(() => fakePageData.Id).MustHaveHappened();
//            A.CallTo(() => fakePageManager.GetPreview(A<Guid>.That.IsEqualTo(dummyCurrentPageId))).MustHaveHappened();
//        }

//        [Fact()]
//        public void GetControlsInOrderTest()
//        {
//            Assert.True(false, "This test needs an implementation");
//        }

//        //[Fact()]
//        public void GetNextControlOnPageTest()
//        {
//            Assert.True(false, "This test needs an implementation");
//        }

//        //[Fact()]
//        public void GetControlOnPageTest()
//        {
//            Assert.True(false, "This test needs an implementation");
//        }

//        //[Fact()]
//        public void GetWidgetsTest()
//        {
//            Assert.True(false, "This test needs an implementation");
//        }
//    }
//}