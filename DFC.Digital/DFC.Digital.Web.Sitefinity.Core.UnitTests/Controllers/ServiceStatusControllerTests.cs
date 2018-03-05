﻿using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests.Controllers
{
    public class ServiceStatusControllerTests
    {
        [Theory]
        [InlineData(ServiceState.Green, 200)]
        [InlineData(ServiceState.Amber, 502)]
        public void IndexDataTest(ServiceState serviceState, int expectedStatusCode)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());

            var fakeWebAppContext = A.Fake<IWebAppContext>();

            //Instantiate & Act
            var serviceStatusController = new ServiceStatusController(GetTestDependentServces(serviceState), loggerFake, fakeWebAppContext);

            //Act
            var indexResult = serviceStatusController.WithCallTo(c => c.Index());

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<ServiceStatusModel>(vm =>
            {
                vm.CheckDateTime.Should().BeCloseTo(DateTime.Now, 100000);
                vm.ServiceStatues.Should().NotBeNullOrEmpty();
                vm.ServiceStatues[0].Name.ShouldBeEquivalentTo("Dummy Service One");
                vm.ServiceStatues[0].Status.ShouldBeEquivalentTo(serviceState);
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

        private IEnumerable<DependencyHealthCheckService> GetTestDependentServces(ServiceState serviceState)
        {
            //Set up a fake service
            var servicesFake = A.Fake<IServiceStatus>(ops => ops.Strict());
            A.CallTo(() => servicesFake.GetCurrentStatusAsync()).Returns(new ServiceStatus { Name = "Dummy Service One", Status = serviceState, Notes = string.Empty });
            yield return new DependencyHealthCheckService(servicesFake);
        }
    }
}
