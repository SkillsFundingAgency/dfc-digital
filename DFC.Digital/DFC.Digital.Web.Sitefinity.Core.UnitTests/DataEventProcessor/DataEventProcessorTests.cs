using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Pages.Model;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class DataEventProcessorTests
    {
        private const string PropertyChangeThatCauseExport = "propertyChangeThatCauseExport";

        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ICompositePageBuilder fakeCompositePageBuilder;
        private readonly IMicroServicesPublishingService fakeCompositeUIService;
        private readonly IDataEventActions fakeDataEventActions;
        private readonly IDataEvent fakeDataEvent;
        private readonly IAsyncHelper fakeAsyncHelper;

        public DataEventProcessorTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeCompositePageBuilder = A.Fake<ICompositePageBuilder>();
            fakeCompositeUIService = A.Fake<IMicroServicesPublishingService>();
            fakeDataEventActions = A.Fake<IDataEventActions>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeDataEvent = A.Fake<IDataEvent>();
        }

        [Theory]
        [InlineData(true, true, MicroServicesDataEventAction.PublishedOrUpdated, "DummyConfigKey")]
        [InlineData(false, false, MicroServicesDataEventAction.PublishedOrUpdated, "DummyConfigKey")]
        [InlineData(false, true, MicroServicesDataEventAction.PublishedOrUpdated, "")]
        [InlineData(false, true, MicroServicesDataEventAction.UnpublishedOrDeleted, "DummyConfigKey")]
        public void ExportCompositePageTests(bool expectDataTobePosted, bool shouldPostPage, MicroServicesDataEventAction microServicesDataEventAction, string microServiceEndPointConfigKey)
        {
            //Setup
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(A.Dummy<string>());
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Returns(microServicesDataEventAction);
            A.CallTo(() => fakeDataEventActions.ShouldExportPage(A<IDataEvent>._)).Returns(shouldPostPage);
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(microServiceEndPointConfigKey);

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeCompositeUIService, fakeAsyncHelper, fakeDataEventActions);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (expectDataTobePosted)
            {
                A.CallTo(() => fakeCompositeUIService.PostPageDataAsync(A<string>._, A<MicroServicesPublishingPageData>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeCompositeUIService.PostPageDataAsync(A<string>._, A<MicroServicesPublishingPageData>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData(true, MicroServicesDataEventAction.UnpublishedOrDeleted, "DummyConfigKey")]
        [InlineData(false, MicroServicesDataEventAction.UnpublishedOrDeleted, "")]
        [InlineData(false, MicroServicesDataEventAction.Ignored, "DummyConfigKey")]
        public void DeleteCompositePageTests(bool expectDataTobePosted, MicroServicesDataEventAction microServicesDataEventAction, string microServiceEndPointConfigKey)
        {
            //Setup
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(A.Dummy<string>());
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Returns(microServicesDataEventAction);
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(microServiceEndPointConfigKey);

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeCompositeUIService, fakeAsyncHelper, fakeDataEventActions);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (expectDataTobePosted)
            {
                A.CallTo(() => fakeCompositeUIService.DeletePageAsync(A<string>._, A<Guid>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeCompositeUIService.DeletePageAsync(A<string>._, A<Guid>._)).MustNotHaveHappened();
            }
        }

        [Fact]
        public void ExportCompositePageExceptionTest()
        {
            //Setup
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Throws(new ApplicationException());

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeCompositeUIService, fakeAsyncHelper, fakeDataEventActions);

            //Asserts
            Assert.Throws<ApplicationException>(() => dataEventHandler.ExportCompositePage(fakeDataEvent));

            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void NullArgumentExceptionTest()
        {
            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeCompositeUIService, fakeAsyncHelper, fakeDataEventActions);

            //Asserts
            Assert.Throws<ArgumentNullException>(() => dataEventHandler.ExportCompositePage(null));
        }
    }
}
