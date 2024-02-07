using System.Threading.Tasks;
using UnityEngine;

public interface IConfigService
{
    T GetConfig<T>(string key);
    void GetConfig<T>(string key, T thing) where T : ScriptableObject;

}