using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IMergeEmailContent
    {
        string MergeTemplateBodyWithContent(ContactAdvisorRequest sendEmailRequest, string content);

        string MergeTemplateBodyWithContentWithHtml(ContactAdvisorRequest sendEmailRequest, string content);
    }
}
