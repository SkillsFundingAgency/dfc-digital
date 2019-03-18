using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public class TechnicalFeedback
    {
        [Required(ErrorMessage = "Enter a message describing the issue")]
        public string Message { get; set; }
    }
}