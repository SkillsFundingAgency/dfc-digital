using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IMergeEmailContent<in T>
        where T : class
    {
        string MergeTemplateBodyWithContent(T sendEmailRequest, string content);
    }
}
