using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IUserRepository
    {
        string GetUserNameById(Guid id);
    }
}
