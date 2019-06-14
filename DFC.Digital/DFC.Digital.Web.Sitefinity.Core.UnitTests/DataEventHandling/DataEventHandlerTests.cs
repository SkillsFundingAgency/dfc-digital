using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Publishing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class DataEventHandlerTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ICompositePageBuilder fakeCompositePageBuilder;
        private readonly ICompositeUIService fakeCompositeUIService;
        private readonly ISitefinityDataEventProxy fakeSitefinityDataEventProxy;
        private readonly IDataEvent fakeDataEvent;
        private readonly IAsyncHelper fakeAsyncHelper;

        public DataEventHandlerTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeCompositePageBuilder = A.Fake<ICompositePageBuilder>();
            fakeCompositeUIService = A.Fake<ICompositeUIService>();
            fakeSitefinityDataEventProxy = A.Fake<ISitefinityDataEventProxy>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();
            fakeDataEvent = A.Fake<IDataEvent>();
        }

        [Theory]
        [InlineData(Constants.ItemActionUpdated, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, true)]
        [InlineData("NotUpdated", Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, false)]
        [InlineData(Constants.ItemActionUpdated, "Not Published", Constants.ItemStatusLive, false)]
        [InlineData(Constants.ItemActionUpdated, Constants.WorkFlowStatusPublished, "NotLive", false)]
        public void ExportCompositePageEventConditionsTest(string action, string workFlowState, string status, bool shouldPostPage)
        {
            //Setup
            SetUp(action, typeof(PageNode), true, workFlowState, status, 1);

            //Act
            var dataEventHandler = new DataEventHandler(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);
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
        [InlineData(typeof(PageNode),  true, 1, true)]
        [InlineData(typeof(PageNode), false, 1, true)]
        [InlineData(typeof(PageNode), true, 0, true)]
        [InlineData(typeof(int), true, 1, false)]
        [InlineData(typeof(PageNode), false, 0, false)]
        public void ExportCompositePageNodeConditionsTest(Type type, bool hasPageChanged, int changedProperites, bool shouldPostPage)
        {
            //Setup
            SetUp(Constants.ItemActionUpdated, type, hasPageChanged, Constants.WorkFlowStatusPublished, Constants.ItemStatusLive, changedProperites);

            //Act
            var dataEventHandler = new DataEventHandler(fakeApplicationLogger, fakeCompositePageBuilder, fakeSitefinityDataEventProxy, fakeCompositeUIService, fakeAsyncHelper);
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

        private void SetUp(string eventAction, Type itemType, bool pageChanged, string workflowStatus, string itemStatus, int numberChangedProperties)
        {
            A.CallTo(() => fakeDataEvent.Action).Returns(eventAction);
            A.CallTo(() => fakeDataEvent.ItemType).Returns(itemType);

            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<bool>(fakeDataEvent, Constants.HasPageDataChanged)).Returns(pageChanged);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ApprovalWorkflowState)).Returns(workflowStatus);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ItemStatus)).Returns(itemStatus);

            var dummyChangedProperties = new Dictionary<string, PropertyChange>();
            for (int ii = 0; ii < numberChangedProperties; ii++)
            {
                dummyChangedProperties.Add($"Description{ii}", new PropertyChange() { NewValue = $"New Text{ii}" });
            }

            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(fakeDataEvent, Constants.ChangedProperties)).Returns(dummyChangedProperties);
        }
    }
}
