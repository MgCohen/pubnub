public interface ISaveService
{
    T GetSave<T>(string key, T defaultValue = default);
}