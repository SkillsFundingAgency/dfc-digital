namespace DFC.Digital.Data.Interfaces
{
    public interface IRecycleBinRepository
    {
        bool DeleteVacanciesPermanently(int itemCount);
    }
}
