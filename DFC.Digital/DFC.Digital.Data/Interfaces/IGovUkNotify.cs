using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    /// <summary>
    /// Gov Uk Notify Service
    /// </summary>
    public interface IGovUkNotify
    {
        /// <summary>
        /// Submits the email.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="vocPersonalisation"> Dictionary to send clientId and jobprofile value to GovUKNotifyEmail</param>
        /// <returns>true if success else false</returns>
        bool SubmitEmail(string emailAddress, VocSurveyPersonalisation vocPersonalisation);

        Dictionary<string, dynamic> Convert(VocSurveyPersonalisation vocSurveyPersonalisation);
    }
}