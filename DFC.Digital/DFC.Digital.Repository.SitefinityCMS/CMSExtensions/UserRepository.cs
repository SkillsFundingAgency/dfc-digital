using DFC.Digital.Data.CMSExtension.Interfaces;
using System;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserRepository : IUserRepository
    {
        public string GetUserNameById(Guid id)
        {
            var userManager = UserManager.GetManager();

            var user = userManager.GetUser(id);

            if (user != null)
            {
                var profileManager = UserProfileManager.GetManager();
                var profile = profileManager.GetUserProfile<SitefinityProfile>(user);
                return profile.Nickname ?? $"{profile.FirstName} {profile.LastName}";
            }

            return "User not found";
        }
    }
}
