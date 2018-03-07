namespace DFC.Digital.Core
{
    public interface IConfigurationProvider
    {
        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);

        void Add<T>(string key, T value);
    }
}