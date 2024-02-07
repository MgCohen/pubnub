public interface ISaveService
{
    void ClearLocal(string key);
    T Load<T>(string key, T defaultValue = default);
    T LoadGameState<T>(string key, T defaultValue = default);
    bool LoadLocal<T>(string key, out T defaultValue);
    void SaveLocal<T>(string key, T content);
}