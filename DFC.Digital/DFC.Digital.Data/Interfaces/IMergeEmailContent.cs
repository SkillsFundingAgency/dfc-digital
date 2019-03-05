namespace DFC.Digital.Data.Interfaces
{
    public interface IMergeEmailContent
    {
        string MergeTemplateBodyWithContent(string templateBody, string content);

        string MergeTemplateBodyWithContentWithHtml(string templateBody, string content);
    }
}
