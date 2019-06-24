using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileStructuredDataViewModel : StructuredDataInjection
    {
        public bool InPreviewMode { get; set; }

        public string DemoScript { get; set; } =
            "<script type=\"application/ld+json\">{\"@context\": \"https://schema.org/\",\"@type\": \"Occupation\",\"name\": \"Care worker\"},}</script>";
    }
}