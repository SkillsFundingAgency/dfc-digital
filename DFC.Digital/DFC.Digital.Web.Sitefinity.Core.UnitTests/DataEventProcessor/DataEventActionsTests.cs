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
    public class DataEventActionsTests
    {
        private readonly ISitefinityDataEventProxy fakeSitefinityDataEventProxy;
        private readonly IDataEvent fakeDataEvent;

        public DataEventActionsTests()
        {
            fakeSitefinityDataEventProxy = A.Fake<ISitefinityDataEventProxy>(ops => ops.Strict());
            fakeDataEvent = A.Fake<IDataEvent>();
        }

        [Theory]
        [InlineData(MicroServicesDataEventAction.PublishedOrUpdated, Constants.ItemActionUpdated, Constants.WorkflowStatusPublished, RecycleBinAction.None)]
        [InlineData(MicroServicesDataEventAction.UnpublishedOrDeleted, Constants.ItemActionDeleted, Constants.WorkflowStatusUnpublished, RecycleBinAction.RestoreFromRecycleBin)]
        [InlineData(MicroServicesDataEventAction.Ignored, Constants.ItemActionUpdated, "", RecycleBinAction.None)]
        public void GetEventActionTest(MicroServicesDataEventAction expectedAction, string eventAction, string workflowState, RecycleBinAction recycleBinAction)
        {
            //Setup
            A.CallTo(() => fakeDataEvent.Action).Returns(eventAction);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ApprovalWorkflowState)).Returns(workflowState);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<RecycleBinAction>(fakeDataEvent, Constants.RecycleBinAction)).Returns(recycleBinAction);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<string>(fakeDataEvent, Constants.ItemStatus)).Returns(Constants.ItemStatusLive);

            //Act
            var dataEventHandler = new DataEventActions(fakeSitefinityDataEventProxy);
            var result = dataEventHandler.GetEventAction(fakeDataEvent);

            //Asserts
            result.Should().Be(expectedAction);
        }

        [Fact]
        public void GetEventActionNullArgumentsTest()
        {
            //Act
            var dataEventHandler = new DataEventActions(fakeSitefinityDataEventProxy);

            //Asserts
            Assert.Throws<ArgumentNullException>(() => dataEventHandler.GetEventAction(null));
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]

        public void ShouldExportPageTest(bool expectedResult, bool propertiesChanged, bool hasPageChanged)
        {
            //Setup
            var dummyChangedProperties = new Dictionary<string, PropertyChange>();
            if (propertiesChanged)
            {
                dummyChangedProperties.Add("PropertyKey", new PropertyChange() { NewValue = $"New Text" });
            }

            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(fakeDataEvent, Constants.ChangedProperties)).Returns(dummyChangedProperties);
            A.CallTo(() => fakeSitefinityDataEventProxy.GetPropertyValue<bool>(fakeDataEvent, Constants.HasPageDataChanged)).Returns(hasPageChanged);

            //Act
            var dataEventHandler = new DataEventActions(fakeSitefinityDataEventProxy);
            var result = dataEventHandler.ShouldExportPage(fakeDataEvent);

            //Asserts
            result.Should().Be(expectedResult);
        }
    }
}
