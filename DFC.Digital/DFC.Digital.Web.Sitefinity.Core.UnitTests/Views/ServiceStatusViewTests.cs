using ASP;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests.Views
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

        private void CheckViewForService(ServiceStatus serviceStatus, HtmlDocument htmlDom, int index)
        {
            //Check class of li
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]").FirstOrDefault().HasClass($"list_service_{serviceStatus.Status}");

            //First col contains Service name
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[1]/div[1]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.Name);

            //Second col contains paramenters name
            htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[1]/div[2]").FirstOrDefault().InnerText.Should().Contain(serviceStatus.CheckParametersUsed);

            //Second row contains extended information
            if (serviceStatus.Notes != string.Empty)
            {
                htmlDom.DocumentNode.SelectNodes($"//html/body/main/div/ul/li[{index}]/div[2]/div").FirstOrDefault().InnerText.Should().Contain(serviceStatus.Notes);
            }
        }

        private IEnumerable<JobProfileRelatedCareer> GetViewAnchorLinks(HtmlDocument htmlDom)
        {
            return htmlDom.DocumentNode.Descendants("li")
                  .Select(n =>
                  {
                      var firstOrDefault = n.Descendants("a").FirstOrDefault();
                      if (firstOrDefault != null)
                      {
                          return new JobProfileRelatedCareer
                          {
                              Title = firstOrDefault.InnerText,
                              ProfileLink = firstOrDefault.GetAttributeValue("href", string.Empty)
                          };
                      }

                      return null;
                  })
                  .ToList();
        }

        private ServiceStatusModel GenerateServiceStatusViewModel()
        {
            return new ServiceStatusModel() { CheckDateTime = DateTime.Now, ServiceStatues = GetDummyStatuses() };
        }

        private List<ServiceStatus> GetDummyStatuses()
        {
            var serviceStates = new List<ServiceStatus>
            {
                new ServiceStatus() { Name = "Dummy Service Green", Status = ServiceState.Green, Notes = string.Empty, CheckParametersUsed = "P1=Green Parameter" },
                new ServiceStatus() { Name = "Dummy Service Amber", Status = ServiceState.Amber, Notes = "Nearly Bad Service", CheckParametersUsed = "P1=Amber Parameter" },
                new ServiceStatus() { Name = "Dummy Service Red", Status = ServiceState.Red, Notes = "Bad Service", CheckParametersUsed = "P1=Red Parameter" }
            };
            return serviceStates;
        }
    }
}
