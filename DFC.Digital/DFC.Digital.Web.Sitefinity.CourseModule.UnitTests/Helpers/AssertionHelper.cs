using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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

        public static void AssertTagInnerTextValueDoesNotExist(HtmlDocument htmlDocument, string innerText, string tag)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(h2 => h2.InnerText.Contains(innerText)).Should().BeFalse();
        }

        public static void AssertOrderOfTextDisplayed(HtmlDocument htmlDocument, IEnumerable<string> textToCheck)
        {
            var orderIndex = 0;
            foreach (var text in textToCheck)
            {
                var textIndex = htmlDocument.DocumentNode.InnerHtml.IndexOf(text, StringComparison.InvariantCultureIgnoreCase);
                textIndex.Should().BeGreaterThan(orderIndex);
                orderIndex = textIndex;
            }
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

        public static void AssertElementExistsByTagAndClassName(HtmlDocument htmlDocument, string tag, string className)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(element => element.Attributes["class"].Value.Contains(className))
                .Should().BeTrue();
        }

        public static void AssertElementExistsByAttributeAndValue(HtmlDocument htmlDocument, string tag, string attribute, string value)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(element => element.Attributes[attribute].Value.Contains(value))
                .Should().BeTrue();
        }

        public static void AssertElementExistsByAttributeAndTypeAndValue(HtmlDocument htmlDocument, string tag, string attribute, string value, string type)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(element => element.Attributes[attribute].Value.Contains(value) && element.Attributes[nameof(type)].Value.Contains(type))
                .Should().BeTrue();
        }

        public static void AssertElementIsSelectedByAttributeAndValue(HtmlDocument htmlDocument, string tag, string attribute, string value)
        {
            htmlDocument.DocumentNode.Descendants(tag)
                .Any(element => element.Attributes[attribute].Value.Contains(value) && element.Attributes["checked"].Value.Contains("checked"))
                .Should().BeTrue();
        }
    }
}
