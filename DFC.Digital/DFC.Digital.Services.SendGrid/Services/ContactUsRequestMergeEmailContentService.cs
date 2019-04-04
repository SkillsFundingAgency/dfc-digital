using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Services.SendGrid
{
    public class ContactUsRequestMergeEmailContentService : IMergeEmailContent<ContactUsRequest>
    {
        private const string FirstNameToken = "{firstname}";
        private const string LastNameToken = "{lastname}";
        private const string EmailToken = "{email}";
        private const string ContactOptionToken = "{contactoption}";
        private const string DobToken = "{dob}";
        private const string PostCodeToken = "{postcode}";
        private const string ContactAdvisorQuestionTypeToken = "{contactadviserquestiontype}";
        private const string MessageToken = "{message}";
        private const string IsContactableToken = "{iscontactable}";
        private const string FeedbackQuestionTypeToken = "{feedbackquestiontype}";
        private const string TermsAndConditionsToken = "{tandc}";

        public string MergeTemplateBodyWithContent(ContactUsRequest sendEmailRequest, string content)
        {
            if (sendEmailRequest == null)
            {
                return content;
            }

            return TokenReplacement(sendEmailRequest, content);
        }

        private static string TokenReplacement(ContactUsRequest sendEmailRequest, string content)
        {
            var mergedContent = content;
            if (!string.IsNullOrEmpty(mergedContent))
            {
                mergedContent = mergedContent.Replace(FirstNameToken, sendEmailRequest.FirstName);
                mergedContent = mergedContent.Replace(LastNameToken, sendEmailRequest.LastName);
                mergedContent = mergedContent.Replace(EmailToken, sendEmailRequest.Email);
                mergedContent = mergedContent.Replace(ContactOptionToken, sendEmailRequest.ContactOption);
                mergedContent = mergedContent.Replace(DobToken, sendEmailRequest.DateOfBirth?.ToString("dd/MM/yyyy"));
                mergedContent = mergedContent.Replace(PostCodeToken, sendEmailRequest.Postcode);
                mergedContent = mergedContent.Replace(ContactAdvisorQuestionTypeToken, sendEmailRequest.ContactAdviserQuestionType);
                mergedContent = mergedContent.Replace(MessageToken, sendEmailRequest.Message.Replace("\r\n", Environment.NewLine));
                mergedContent = mergedContent.Replace(IsContactableToken, sendEmailRequest.IsContactable.ToString());
                mergedContent = mergedContent.Replace(FeedbackQuestionTypeToken, sendEmailRequest.FeedbackQuestionType);
                mergedContent = mergedContent.Replace(TermsAndConditionsToken, sendEmailRequest.TermsAndConditions.ToString());
            }

            return mergedContent;
        }
    }
}
