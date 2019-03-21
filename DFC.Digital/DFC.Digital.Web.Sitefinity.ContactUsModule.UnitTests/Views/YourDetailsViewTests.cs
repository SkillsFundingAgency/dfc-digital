namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    //public class YourDetailsViewTests
    //{
    //    [Theory]
    //    [InlineData(false)]
    //    [InlineData(true)]
    //    public void Dfc7630YourDetailsViewTests(bool contactAdviser)
    //    {
    //        // Arrange
    //        var yourDetailsIndex = new _MVC_Views_YourDetails_ContactAdvisor_cshtml();
    //        var contactUsViewModel = new ContactUsWithDobPostcodeViewModel();

    //        // Act
    //        var htmlDocument = yourDetailsIndex.RenderAsHtml(contactUsViewModel);

    //        // Assert
    //        if (contactAdviser)
    //        {
    //            AssertDobAndPostCodeExistsInView(htmlDocument);
    //        }
    //        else
    //        {
    //            AssertIsContactableExistsInView(htmlDocument);
    //        }
    //    }

    //    [Theory]
    //    [InlineData(true)]
    //    [InlineData(false)]
    //    public void Dfc7630ErrorSummaryViewTests(bool modelStateInvalid)
    //    {
    //        // Arrange
    //        var errorSummaryView = new _MVC_Views_Shared_ErrorSummary_cshtml();

    //        if (modelStateInvalid)
    //        {
    //            errorSummaryView.ViewData.ModelState.AddModelError(nameof(DateOfBirthPostcodeDetails.Firstname), nameof(Exception.Message));
    //        }

    //        // Act
    //        var htmlDocument = errorSummaryView.RenderAsHtml();

    //        // Assert
    //        if (modelStateInvalid)
    //        {
    //            AssertErrorDetailInSummary(htmlDocument, nameof(Exception.Message));
    //        }
    //        else
    //        {
    //            AssertViewIsEmpty(htmlDocument);
    //        }
    //    }

    //    private static void AssertViewIsEmpty(HtmlDocument htmlDocument)
    //    {
    //        htmlDocument.DocumentNode.Descendants().Count().Should().Be(0);
    //    }

    //    private void AssertErrorDetailInSummary(HtmlDocument htmlDocument, string errorMessage)
    //    {
    //        htmlDocument.DocumentNode.Descendants("h2")
    //            .Count(h2 => h2.InnerText.Contains("There is a problem")).Should().BeGreaterThan(0);
    //        htmlDocument.DocumentNode.Descendants("a")
    //            .Count(a => a.InnerText.Contains(errorMessage)).Should().BeGreaterThan(0);

    //    }

    //    private void AssertIsContactableExistsInView(HtmlDocument htmlDocument)
    //    {
    //        htmlDocument.DocumentNode.Descendants("h2")
    //            .Count(h2 => h2.InnerText.Contains("Do you want us to contact you?")).Should().BeGreaterThan(0);
    //        htmlDocument.DocumentNode.Descendants("span")
    //            .Count(h2 => h2.Id.Equals("dob-hint")).Should().Be(0);
    //        htmlDocument.DocumentNode.Descendants("span")
    //            .Count(h2 => h2.Id.Equals("postcode-hint")).Should().Be(0);
    //    }

    //    private void AssertDobAndPostCodeExistsInView(HtmlDocument htmlDocument)
    //    {
    //        htmlDocument.DocumentNode.Descendants("span")
    //            .Count(h2 => h2.Id.Equals("dob-hint")).Should().BeGreaterThan(0);
    //        htmlDocument.DocumentNode.Descendants("span")
    //            .Count(h2 => h2.Id.Equals("postcode-hint")).Should().BeGreaterThan(0);
    //        htmlDocument.DocumentNode.Descendants("h2")
    //            .Count(h2 => h2.InnerText.Contains("Do you want us to contact you?")).Should().Be(0);
    //    }
    //}
}
