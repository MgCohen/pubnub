using System.Threading.Tasks;

public interface IConfigService
{
    T GetConfig<T>(string key);
}