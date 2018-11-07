using DFC.Digital.Data.CMSExtension.Interfaces;
using System;
using Telerik.Sitefinity.Security;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class UserRepository : IUserRepository
    {
        public string GetUserNameById(Guid id)
        {
            var userManager = UserManager.GetManager();

            var user = userManager.GetUser(id);

            return user == null ? "User not found" : user.UserName;
        }
    }
}
