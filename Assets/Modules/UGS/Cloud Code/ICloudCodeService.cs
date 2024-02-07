using System;
using System.Threading.Tasks;

public interface ICloudCodeService
{
    Task<ModuleResponse<T>> Request<T>(PubnubRequest request);
    void Request<T>(PubnubRequest request, Action<T> callback, Action<string> error);
    Task<ModuleResponse> Request(PubnubRequest request);
}