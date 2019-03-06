using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IMergeEmailContent
    {
        string MergeTemplateBodyWithContent(SendEmailRequest sendEmailRequest, string content);

        string MergeTemplateBodyWithContentWithHtml(SendEmailRequest sendEmailRequest, string content);
    }
}
