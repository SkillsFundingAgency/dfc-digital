﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using FakeItEasy;
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
        private readonly ISitefinityDataEventProxy fakeSitefinityDataEventProxy;
        private readonly IDataEvent fakeDataEvent;
        private readonly IAsyncHelper fakeAsyncHelper;

        public DataEventProcessorTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeCompositePageBuilder = A.Fake<ICompositePageBuilder>();
            fakeCompositeUIService = A.Fake<IMicroServicesPublishingService>();
            fakeSitefinityDataEventProxy = A.Fake<ISitefinityDataEventProxy>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeDataEvent = A.Fake<IDataEvent>();
        }

        [Theory]
        [InlineData(Constants.ItemActionUpdated, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, true)]
        [InlineData("NotUpdated", Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, false)]
        [InlineData(Constants.ItemActionUpdated, "Not Published", Constants.ItemStatusLive, false)]
        [InlineData(Constants.ItemActionUpdated, Constants.WorkFlowStatusPublished, "NotLive", false)]
        public void ExportCompositePageEventConditionsTest(string action, string workflowState, string status, bool shouldPostPage)
        {
            //Setup
            SetUp(action, typeof(PageNode), true, workflowState, status, PropertyChangeThatCauseExport);
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns("DummyMicroServiceEndPointConfigKey");

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (shouldPostPage)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData(typeof(PageNode),  true, PropertyChangeThatCauseExport, true)]
        [InlineData(typeof(PageNode), false, PropertyChangeThatCauseExport, true)]
        [InlineData(typeof(PageNode), true, Constants.ApprovalWorkflowState, true)]
        [InlineData(typeof(int), true, PropertyChangeThatCauseExport, false)]
        [InlineData(typeof(PageNode), false, Constants.ApprovalWorkflowState, false)]
        public void ExportCompositePageNodeConditionsTest(Type type, bool hasPageChanged, string changedProperitesKey, bool shouldPostPage)
        {
            //Setup
            SetUp(Constants.ItemActionUpdated, type, hasPageChanged, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, changedProperitesKey);
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns("DummyMicroServiceEndPointConfigKey");

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (shouldPostPage)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("KeyNameIsSetUp", true)]
        public void MicroServiceEndPointConfigKeyTest(string microServiceEndPointConfigKey, bool shouldPostPage)
        {
            //Setup
            SetUp(Constants.ItemActionUpdated, typeof(PageNode), true, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, PropertyChangeThatCauseExport);
            A.CallTo(() => fakeCompositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(A<string>._, A<Type>._, A<Guid>._)).Returns(microServiceEndPointConfigKey);

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);
            dataEventHandler.ExportCompositePage(fakeDataEvent);

            //Asserts
            if (shouldPostPage)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetCompositePageForPageNode(A<string>._, A<Type>._, A<Guid>._)).MustNotHaveHappened();
            }
        }

        [Fact]
        public void ExportCompositePageExceptionTest()
        {
            //Setup
            SetUp(Constants.ItemActionUpdated, typeof(PageNode), true, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, PropertyChangeThatCauseExport);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<bool>(fakeDataEvent, Constants.HasPageDataChanged)).Throws(new ApplicationException());
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();

            //Act
            var dataEventHandler = new DataEventProcessor(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);

            //Asserts
            Assert.Throws<ApplicationException>(() => dataEventHandler.ExportCompositePage(fakeDataEvent));
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappenedOnceExactly();
        }

        private void SetUp(string eventAction, Type itemType, bool pageChanged, string workflowStatus, string itemStatus, string changedPropertiesKey)
        {
            A.CallTo(() => fakeDataEvent.Action).Returns(eventAction);
            A.CallTo(() => fakeDataEvent.ItemType).Returns(itemType);

            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<bool>(fakeDataEvent, Constants.HasPageDataChanged)).Returns(pageChanged);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ApprovalWorkflowState)).Returns(workflowStatus);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ItemStatus)).Returns(itemStatus);

            var dummyChangedProperties = new Dictionary<string, PropertyChange>();
            dummyChangedProperties.Add(changedPropertiesKey, new PropertyChange() { NewValue = $"New Text" });

            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(fakeDataEvent, Constants.ChangedProperties)).Returns(dummyChangedProperties);
        }
    }
}