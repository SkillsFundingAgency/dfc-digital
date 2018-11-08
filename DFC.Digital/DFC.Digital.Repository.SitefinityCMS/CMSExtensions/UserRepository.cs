using DFC.Digital.Data.CMSExtension.Interfaces;
using System;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class UserRepository : IUserRepository
    {
        public string GetUserNameById(Guid id)
        {
            var userManager = UserManager.GetManager();
            UserProfileManager profileManager = UserProfileManager.GetManager();

            var user = userManager.GetUser(id);

            if (user != null)
            {
                var profile = profileManager.GetUserProfile<SitefinityProfile>(user);
                return profile.Nickname ?? $"{profile.FirstName} {profile.LastName}";
            }

            return "User not found";
        }
    }
}
