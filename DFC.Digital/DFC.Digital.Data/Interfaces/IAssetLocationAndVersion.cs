using System.Web;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAssetLocationAndVersion
    {
        string GetLocationAssetFileAndVersion(string fileName);
    }
}
