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

            var sectionText = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-medium')]").FirstOrDefault().InnerText;
            sectionText.Should().BeEquivalentTo("Service Status");

            var checkDate = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-small')]").FirstOrDefault().InnerText;
            checkDate.Should().Contain(serviceStatusViewModel.CheckDateTime.ToString("dd/MM/yyyy hh:mm:ss"));

            //The li of each element matches the state of the service
            for (int ii = 0; ii < serviceStatusViewModel.ServiceStatues.Count(); ii++)
            {
                CheckViewForService(serviceStatusViewModel.ServiceStatues[ii], htmlDom, ii + 1);
            }
        }

        private void CheckViewForService(ServiceStatusModel serviceStatus, HtmlDocument htmlDom, int index)
        {
            //Check class of li
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]").FirstOrDefault().GetAttributeValue("class", string.Empty).Contains($"list_service_{serviceStatus.Status}");

            //First col contains Service name
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[1]/div[1]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.Name);

            //Second col contains status text and colleration id
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[1]/div[2]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.StatusText);
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[1]/div[2]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.CheckCorrelationId);
        }

        private ServiceStatuesModel GenerateServiceStatusViewModel()
        {
            return new ServiceStatuesModel() { CheckDateTime = DateTime.Now, ServiceStatues = GetDummyStatuses() };
        }

        private Collection<ServiceStatusModel> GetDummyStatuses()
        {
            var serviceStatusModel = new Collection<ServiceStatusModel>
            {
                new ServiceStatusModel() { Name = "Dummy Service Green", Status = ServiceState.Green, StatusText = "Available", CheckCorrelationId = "DummyGuid" },
                new ServiceStatusModel() { Name = "Dummy Service Amber", Status = ServiceState.Amber, StatusText = "Degraded", CheckCorrelationId = "DummyGuid" },
                new ServiceStatusModel() { Name = "Dummy Service Red", Status = ServiceState.Red, StatusText = "Unavailable",  CheckCorrelationId = "DummyGuid" }
            };
            return serviceStatusModel;
        }
    }
}
