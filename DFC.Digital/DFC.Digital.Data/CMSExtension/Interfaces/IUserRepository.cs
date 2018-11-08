using System;

namespace DFC.Digital.Data.CMSExtension.Interfaces
{
    public interface IUserRepository
    {
        string GetUserNameById(Guid id);
    }
}
