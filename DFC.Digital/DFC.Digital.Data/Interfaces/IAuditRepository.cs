namespace DFC.Digital.Data.Interfaces
{
    public interface IAuditRepository
    {
        void CreateAudit(object record);
    }
}