using ASP;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class ServiceStatusViewTests
    {
        [Fact]
        public void ServiceStatusViewTest()
        {
            var index = new _MVC_Views_ServiceStatus_Index_cshtml();
            var serviceStatusViewModel = GenerateServiceStatusViewModel();

            var htmlDom = index.RenderAsHtml(serviceStatusViewModel);

            var sectionText = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'govuk-heading-m')]").FirstOrDefault().InnerText;
            sectionText.Should().BeEquivalentTo("Service Status");

            var checkDate = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'govuk-heading-s')]").FirstOrDefault().InnerText;
            checkDate.Should().Contain(serviceStatusViewModel.CheckDateTime.ToString("dd/MM/yyyy hh:mm:ss"));

            //The li of each element matches the state of the service
            for (int ii = 0; ii < serviceStatusViewModel.ServiceStatues.Count(); ii++)
            {
                CheckViewForService(serviceStatusViewModel.ServiceStatues[ii], htmlDom, ii + 1);
            }
        }

        [Fact]
        public void ServiceStatusChildAppsViewTest()
        {
            var index = new _MVC_Views_ServiceStatus_Index_cshtml();
            var serviceStatusViewModel = GenerateServiceStatusViewModel();

            serviceStatusViewModel.ServiceStatues[1].ChildAppStatuses = new List<ServiceStatusChildModel>
            {
                new ServiceStatusChildModel() { Name = "Dummy Service Green", StatusText = "Available" },
                new ServiceStatusChildModel() { Name = "Dummy Service Amber", StatusText = "Degraded" },
                new ServiceStatusChildModel() { Name = "Dummy Service Red",   StatusText = "Unavailable" }
            };

            var htmlDom = index.RenderAsHtml(serviceStatusViewModel);

            //The li of each element matches the state of the service
            for (int ii = 0; ii < serviceStatusViewModel.ServiceStatues[1].ChildAppStatuses.Count(); ii++)
            {
                CheckViewForChildAppService(serviceStatusViewModel.ServiceStatues[1].ChildAppStatuses[ii], htmlDom, ii + 1);
            }
        }

        private void CheckViewForChildAppService(ServiceStatusChildModel serviceChildStatus, HtmlDocument htmlDom, int index)
        {
            var node = htmlDom.DocumentNode.SelectNodes($"//html/body/div/main/div/ul/li[2]/div/div[1]/ul/li[{index}]").FirstOrDefault();

            //First col contains child service name and status
            node.InnerText.Should().Contain(serviceChildStatus.Name);
            node.InnerText.Should().Contain(serviceChildStatus.StatusText);
        }

        private void CheckViewForService(ServiceStatusModel serviceStatus, HtmlDocument htmlDom, int index)
        {
            //Check class of li
            htmlDom.DocumentNode.SelectNodes($"//html/body/div/main/div/ul/li[{index}]").FirstOrDefault().GetAttributeValue("class", string.Empty).Contains($"list_service_{serviceStatus.Status}");

            //First col contains Service name
            htmlDom.DocumentNode.SelectNodes($"//html/body/div/main/div/ul/li[{index}]/div[1]/div[1]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.Name);

            //Second col contains status text and colleration id
            htmlDom.DocumentNode.SelectNodes($"//html/body/div/main/div/ul/li[{index}]/div[1]/div[2]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.StatusText);
            htmlDom.DocumentNode.SelectNodes($"//html/body/div/main/div/ul/li[{index}]/div[1]/div[2]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.CheckCorrelationId);
        }

        private ServiceStatuesModel GenerateServiceStatusViewModel()
        {
            return new ServiceStatuesModel() { CheckDateTime = DateTime.Now, ServiceStatues = GetDummyStatuses() };
        }

        private Collection<ServiceStatusModel> GetDummyStatuses()
        {
            var serviceStatusModel = new Collection<ServiceStatusModel>
            {
                new ServiceStatusModel() { Name = "Dummy Service Green", Status = ServiceState.Green, StatusText = "Available", CheckCorrelationId = "DummyGuid", ChildAppStatuses = new List<ServiceStatusChildModel>() },
                new ServiceStatusModel() { Name = "Dummy Service Amber", Status = ServiceState.Amber, StatusText = "Degraded", CheckCorrelationId = "DummyGuid", ChildAppStatuses = new List<ServiceStatusChildModel>() },
                new ServiceStatusModel() { Name = "Dummy Service Red", Status = ServiceState.Red, StatusText = "Unavailable",  CheckCorrelationId = "DummyGuid", ChildAppStatuses = new List<ServiceStatusChildModel>() }
            };
            return serviceStatusModel;
        }
    }
}
