using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class VocSurveyPersonalisation
    {
        public VocSurveyPersonalisation()
        {
            Personalisation = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Personalisation { get; set; }
    }
}