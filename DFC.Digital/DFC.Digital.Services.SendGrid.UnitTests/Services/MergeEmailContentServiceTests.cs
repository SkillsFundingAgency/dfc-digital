using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class MergeEmailContentServiceTests
    {
        private readonly ContactAdvisorRequest sendEmailRequest;

        public MergeEmailContentServiceTests()
        {
            sendEmailRequest = new ContactAdvisorRequest
            {
                TemplateName = nameof(ContactAdvisorRequest.TemplateName),
                FirstName = nameof(ContactAdvisorRequest.FirstName),
                LastName = nameof(ContactAdvisorRequest.LastName),
                Email = nameof(ContactAdvisorRequest.Email),
                ContactOption = nameof(ContactAdvisorRequest.ContactOption),
                DateOfBirth = new DateTime(2000, 3, 12),
                PostCode = nameof(ContactAdvisorRequest.PostCode),
                ContactAdviserQuestionType = nameof(ContactAdvisorRequest.ContactAdviserQuestionType),
                Message = nameof(ContactAdvisorRequest.Message),
                IsContactable = true,
                FeedbackQuestionType = nameof(ContactAdvisorRequest.FeedbackQuestionType),
                TermsAndConditions = true
            };
        }

        [Theory]
        [InlineData("Contact from {firstname},{lastname}, with email address {email},with Post code {postcode}", "Contact from FirstName,LastName, with email address Email,with Post code PostCode")]
        [InlineData("My name is {firstname}", "My name is FirstName")]
        [InlineData("{firstname} {lastname}, with email address: {email} who was born on {dob}", "FirstName LastName, with email address: Email who was born on 12/03/2000")]
        [InlineData(
            "{postcode} allows us to {contactadviserquestiontype}, delivery message is {message}, is contactable is {iscontactable} has accepted the T&C's is {tandc}",
            "PostCode allows us to ContactAdviserquestionType, delivery message is Message, is contactable is True has accepted the T&C's is True")]
        [InlineData(
            "Random {dob}, for  {postcode}, with {contactadviserquestiontype}, sends {message}, with is contactable as {iscontactable}",
            "Random 12/03/2000, for  PostCode, with ContactAdviserquestionType, sends Message, with is contactable as True")]
        public void MergeTemplateBodyWithContentTest(string templateBody, string expectedMergeData)
        {
            var template = new EmailTemplate
            {
                Body = templateBody
            };
            var mergeEmailContentService = new MergeEmailContentService();

            var mergedData = mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.Body);

            mergedData.Should().BeEquivalentTo(expectedMergeData);
        }

        [Theory]
        [InlineData("Contact from {firstname},{lastname}, with email address {email},with Post code {postcode}", "Contact from FirstName,LastName, with email address Email,with Post code PostCode")]
        [InlineData("My name is {firstname}", "My name is FirstName")]
        [InlineData("{firstname} {lastname}, with email address: {email} who was born on {dob}", "FirstName LastName, with email address: Email who was born on 12/03/2000")]
        [InlineData(
            "{postcode} allows us to {contactadviserquestiontype}, delivery message is {message}, is contactable is {iscontactable} has accepted the T&C's is {tandc}",
            "PostCode allows us to ContactAdviserquestionType, delivery message is Message, is contactable is True has accepted the T&C's is True")]
        [InlineData(
            "Random {dob}, for  {postcode}, with {contactadviserquestiontype}, sends {message}, with is contactable as {iscontactable}",
            "Random 12/03/2000, for  PostCode, with ContactAdviserquestionType, sends Message, with is contactable as True")]
        public void MergeTemplateBodyWithContentWithHtmlTest(string templateBody, string expectedMergeData)
        {
            var template = new EmailTemplate
            {
                Body = templateBody
            };
            var mergeEmailContentService = new MergeEmailContentService();

            var mergedData =
                mergeEmailContentService.MergeTemplateBodyWithContentWithHtml(sendEmailRequest, template.Body);

            mergedData.Should().BeEquivalentTo(expectedMergeData);
        }
    }
}