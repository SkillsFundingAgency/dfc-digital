using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Services.SendGrid
{
    public class SendGridMergeEmailContentService : IMergeEmailContent<ContactUsRequest>
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

        public string MergeTemplateBodyWithContentWithHtml(ContactUsRequest sendEmailRequest, string content)
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

            mergedContent = mergedContent.Replace(FirstNameToken, sendEmailRequest.FirstName);
            mergedContent = mergedContent.Replace(LastNameToken, sendEmailRequest.LastName);
            mergedContent = mergedContent.Replace(EmailToken, sendEmailRequest.Email);
            mergedContent = mergedContent.Replace(ContactOptionToken, sendEmailRequest.ContactOption);
            mergedContent = mergedContent.Replace(DobToken, sendEmailRequest.DateOfBirth.ToString("dd/MM/yyyy"));
            mergedContent = mergedContent.Replace(PostCodeToken, sendEmailRequest.PostCode);
            mergedContent = mergedContent.Replace(ContactAdvisorQuestionTypeToken, sendEmailRequest.ContactAdviserQuestionType);
            mergedContent = mergedContent.Replace(MessageToken, sendEmailRequest.Message);
            mergedContent = mergedContent.Replace(IsContactableToken, sendEmailRequest.IsContactable.ToString());
            mergedContent = mergedContent.Replace(FeedbackQuestionTypeToken, sendEmailRequest.FeedbackQuestionType);
            mergedContent = mergedContent.Replace(TermsAndConditionsToken, sendEmailRequest.TermsAndConditions.ToString());

            return mergedContent;
        }
    }
}
