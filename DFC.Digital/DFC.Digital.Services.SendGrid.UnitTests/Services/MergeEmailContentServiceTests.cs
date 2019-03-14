using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class MergeEmailContentServiceTests
    {
        private readonly ContactUsRequest sendEmailRequest;

        public MergeEmailContentServiceTests()
        {
            sendEmailRequest = new ContactUsRequest
            {
                TemplateName = nameof(ContactUsRequest.TemplateName),
                FirstName = nameof(ContactUsRequest.FirstName),
                LastName = nameof(ContactUsRequest.LastName),
                Email = nameof(ContactUsRequest.Email),
                ContactOption = nameof(ContactUsRequest.ContactOption),
                DateOfBirth = new DateTime(2000, 3, 12),
                PostCode = nameof(ContactUsRequest.PostCode),
                ContactAdviserQuestionType = nameof(ContactUsRequest.ContactAdviserQuestionType),
                Message = nameof(ContactUsRequest.Message),
                IsContactable = true,
                FeedbackQuestionType = nameof(ContactUsRequest.FeedbackQuestionType),
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
        [InlineData(
            "<p>Random {dob}, for  {postcode}, with {contactadviserquestiontype}, sends {message}, with is contactable as {iscontactable}</p>",
            "<p>Random 12/03/2000, for  PostCode, with ContactAdviserquestionType, sends Message, with is contactable as True</p>")]
        public void MergeTemplateBodyWithContentTest(string templateBody, string expectedMergeData)
        {
            var template = new EmailTemplate
            {
                Body = templateBody
            };

            var mergeEmailContentService = new ContactUsRequestMergeEmailContentService();
            var mergedData = mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.Body);

            mergedData.Should().BeEquivalentTo(expectedMergeData);
        }
    }
}