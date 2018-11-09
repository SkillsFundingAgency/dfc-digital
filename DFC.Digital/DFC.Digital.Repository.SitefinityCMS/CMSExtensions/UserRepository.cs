using DFC.Digital.Data.CMSExtension.Interfaces;
using System;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserRepository : IUserRepository, IDisposable
    {
        private UserManager userManager;
        private UserProfileManager userProfileManager;

        public UserRepository()
        {
            userManager = UserManager.GetManager();
            userProfileManager = UserProfileManager.GetManager();
        }

        public string GetUserNameById(Guid id)
        {
            var user = userManager.GetUser(id);
            if (user != null)
            {
                var profile = userProfileManager.GetUserProfile<SitefinityProfile>(user);
                return profile.Nickname ?? $"{profile.FirstName} {profile.LastName}";
            }

            return "User not found";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (userProfileManager != null)
                {
                    userProfileManager.Dispose();
                    userProfileManager = null;
                }
            }
        }
    }
}