using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
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
        private readonly ISitefinityManagerProxy fakeSitefinityManagerProxy;
        private readonly IDataEventActions fakeDataEventActions;
        private readonly IDataEvent fakeDataEvent;
        private readonly IAsyncHelper fakeAsyncHelper;
        private readonly IServiceBusMessageProcessor fakeServiceBusMessageProcessor;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly IDynamicModuleConverter<JobProfileMessage> fakeDynamicContentConverter;
        private readonly IDynamicContentAction fakeDynamicContentAction;

        public DataEventProcessorTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
            fakeCompositePageBuilder = A.Fake<ICompositePageBuilder>();
            fakeDataEventActions = A.Fake<IDataEventActions>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeDataEvent = A.Fake<IDataEvent>();
            fakeSitefinityManagerProxy = A.Fake<ISitefinityManagerProxy>();
            fakeServiceBusMessageProcessor = A.Fake<IServiceBusMessageProcessor>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentConverter = A.Fake<IDynamicModuleConverter<JobProfileMessage>>();
            fakeDynamicContentAction = A.Fake<IDynamicContentAction>();
            A.CallTo(() => fakeDataEvent.ItemType).Returns(typeof(PageNode));
        }

        [Theory]
        [InlineData(true, MicroServicesDataEventAction.PublishedOrUpdated)]
        [InlineData(false, MicroServicesDataEventAction.UnpublishedOrDeleted)]

        public void ExportCompositePageTests(bool isContentPage, MicroServicesDataEventAction microServicesDataEventAction)
        {
            //Setup
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Returns(microServicesDataEventAction);
            if (isContentPage)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetContentPageTypeFromPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(true);
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetContentPageTypeFromPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(false);
            }

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeAsyncHelper, fakeDataEventActions, fakeDynamicContentConverter, fakeServiceBusMessageProcessor, fakeDynamicContentExtensions, fakeDynamicContentAction, fakeSitefinityManagerProxy);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (isContentPage)
            {
                A.CallTo(() => fakeServiceBusMessageProcessor.SendContentPageMessage(A<MicroServicesPublishingPageData>._, A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeServiceBusMessageProcessor.SendContentPageMessage(A<MicroServicesPublishingPageData>._, A<string>._, A<string>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData(true, MicroServicesDataEventAction.UnpublishedOrDeleted)]
        [InlineData(false, MicroServicesDataEventAction.UnpublishedOrDeleted)]
        public void DeleteCompositePageTests(bool isContentPage, MicroServicesDataEventAction microServicesDataEventAction)
        {
            //Setup
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Returns(microServicesDataEventAction);
            if (isContentPage)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetContentPageTypeFromPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(true);
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetContentPageTypeFromPageNode(A<Type>._, A<Guid>._, A<string>._)).Returns(false);
            }

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeAsyncHelper, fakeDataEventActions, fakeDynamicContentConverter, fakeServiceBusMessageProcessor, fakeDynamicContentExtensions, fakeDynamicContentAction, fakeSitefinityManagerProxy);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (isContentPage)
            {
                A.CallTo(() => fakeServiceBusMessageProcessor.SendContentPageMessage(A<MicroServicesPublishingPageData>._, A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeServiceBusMessageProcessor.SendContentPageMessage(A<MicroServicesPublishingPageData>._, A<string>._, A<string>._)).MustNotHaveHappened();
            }
        }

        [Fact]
        public void ExportCompositePageExceptionTest()
        {
            //Setup
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Throws(new ArgumentException());

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeAsyncHelper, fakeDataEventActions, fakeDynamicContentConverter, fakeServiceBusMessageProcessor, fakeDynamicContentExtensions, fakeDynamicContentAction, fakeSitefinityManagerProxy);

            //Asserts
            Assert.Throws<ArgumentException>(() => dataEventHandler.ExportCompositePage(fakeDataEvent));

            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void NullArgumentExceptionTest()
        {
            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeAsyncHelper, fakeDataEventActions, fakeDynamicContentConverter, fakeServiceBusMessageProcessor, fakeDynamicContentExtensions, fakeDynamicContentAction, fakeSitefinityManagerProxy);

            //Asserts
            Assert.Throws<ArgumentNullException>(() => dataEventHandler.ExportCompositePage(null));
        }

        [Theory]
        [InlineData(true, typeof(PageNode))]
        [InlineData(false, null)]
        public void PageNodeTest(bool expectToProcess, Type itemType)
        {
            //Setup
            A.CallTo(() => fakeDataEvent.ItemType).Returns(itemType);
            A.CallTo(() => fakeDataEventActions.GetEventAction(A<IDataEvent>._)).Returns(MicroServicesDataEventAction.Ignored);

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeAsyncHelper, fakeDataEventActions, fakeDynamicContentConverter, fakeServiceBusMessageProcessor, fakeDynamicContentExtensions, fakeDynamicContentAction, fakeSitefinityManagerProxy);
            dataEventHandler.ExportContentData(fakeDataEvent);

            //Asserts
            if (expectToProcess)
            {
                A.CallTo(() => fakeDataEventActions.GetEventAction(fakeDataEvent)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeDataEventActions.GetEventAction(fakeDataEvent)).MustNotHaveHappened();
            }
        }
    }
}
