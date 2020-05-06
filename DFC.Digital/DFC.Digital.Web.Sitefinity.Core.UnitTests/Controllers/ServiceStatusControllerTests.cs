using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class ServiceStatusControllerTests
    {
        [Theory]
        [InlineData(ServiceState.Green, "Avaialble", 200)]
        [InlineData(ServiceState.Amber, "Degraded", 502)]
        public void IndexDataTest(ServiceState serviceState, string serviceText,  int expectedStatusCode)
        {
            //Setup the fakes and dummies
            var fakeWebAppContext = A.Fake<IWebAppContext>();
            var fakeMapper = A.Fake<IMapper>();

            var testServiceStatusModel = new ServiceStatusModel() { Name = "Dummy Service", Status = serviceState, StatusText = serviceText, CheckCorrelationId = "DummyGuid" };

            A.CallTo(() => fakeMapper.Map<ServiceStatusModel>(A<ServiceStatus>.Ignored)).Returns(testServiceStatusModel);

            //Instantiate & Act
            var serviceStatusController = new ServiceStatusController(GetTestDependentServces(testServiceStatusModel), fakeWebAppContext, fakeMapper);

            //Act
            var indexResult = serviceStatusController.WithCallTo(c => c.Index());

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<ServiceStatuesModel>(vm =>
            {
                vm.CheckDateTime.Should().BeCloseTo(DateTime.Now, 100000);
                vm.ServiceStatues.Should().NotBeNullOrEmpty();
                vm.ServiceStatues[0].Should().BeEquivalentTo(testServiceStatusModel);
            }).AndNoModelErrors();

            if (expectedStatusCode == 200)
            {
                A.CallTo(() => fakeWebAppContext.SetResponseStatusCode(A<int>.That.IsEqualTo(expectedStatusCode))).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeWebAppContext.SetResponseStatusCode(A<int>.That.IsEqualTo(expectedStatusCode))).MustHaveHappened();
            }
        }

        private IEnumerable<DependencyHealthCheckService> GetTestDependentServces(ServiceStatusModel testServiceStatusModel)
        {
            //Set up a fake service
            var servicesFake = A.Fake<IServiceStatus>(ops => ops.Strict());
            A.CallTo(() => servicesFake.GetCurrentStatusAsync()).Returns(new ServiceStatus { Name = testServiceStatusModel.Name, Status = testServiceStatusModel.Status, CheckCorrelationId = testServiceStatusModel.Status == ServiceState.Green ? Guid.Empty : Guid.NewGuid() });
            yield return new DependencyHealthCheckService(servicesFake);
        }
    }
}