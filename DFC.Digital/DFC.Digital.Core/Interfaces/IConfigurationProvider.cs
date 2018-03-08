namespace DFC.Digital.Core
{
    public interface IConfigurationProvider
    {
        T GetConfig<T>(string key);

        T GetConfig<T>(string key, T defaultValue);

        void Add<T>(string key, T value);
    }
}