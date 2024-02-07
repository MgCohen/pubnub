using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameModule
{
    public string Key { get; }

    public Task Initialize(GameData gameModules);
}

public abstract class GameModule<T>: IGameModule where T: IGameModuleData
{
    public abstract string Key { get; }

    public async Task Initialize(GameData gameModules)
    {
        T data = gameModules.GetModuleData<T>();
        await Initialize(data);
    }

    protected abstract Task Initialize(T data);

}