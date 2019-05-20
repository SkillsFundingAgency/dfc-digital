using FluentAssertions;
using HtmlAgilityPack;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers
{
    public class AssertionHelper
    {
        public static void AssertTagInnerTextValue(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h2 => h2.InnerText.Contains(innerText)).Should().BeTrue();
        }

        public static void AssertFirstTagInnerTextValue(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .FirstOrDefault()?.InnerText.Should().Contain(innerText);
        }

        public static void AssertTagInnerTextValueDoesNotExist(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h2 => h2.InnerText.Contains(innerText)).Should().BeFalse();
        }

        public static void AssertExistsElementIdWithInnerHtml(HtmlDocument htmlDocument, string id)
        {
            htmlDocument.DocumentNode.Descendants()
                .FirstOrDefault(element => element.Id.Equals(id))
                ?.InnerHtml.Should().NotBeEmpty();
        }

        public static void AssertExistsElementClassWithInnerHtml(HtmlDocument htmlDocument, string className)
        {
            htmlDocument.DocumentNode.Descendants()
                .FirstOrDefault(element => element.Attributes["class"].Value.Contains(className))
                ?.InnerHtml.Should().NotBeEmpty();
        }

        public static void AssertElementDoesNotExistsById(HtmlDocument htmlDocument, string id)
        {
            htmlDocument.DocumentNode.Descendants()
                .Any(element => element.Id.Equals(id))
                .Should().BeFalse();
        }
    }
}
