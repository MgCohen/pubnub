using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

public class CloudCodeService : IUnityService, ICloudCodeService
{
    public Task Initialize()
    {
        return Task.CompletedTask;
    }

    public async Task<ModuleResponse> Request(PubnubRequest request)
    {
        Debug.Log($"Started Request {request.Module} - {request.Endpoint}");
        Dictionary<string, object> args = JObject.FromObject(request).ToObject<Dictionary<string, object>>();
        string result = string.Empty;
        string error = string.Empty;
        try
        {
            result = await Unity.Services.CloudCode.CloudCodeService.Instance.CallModuleEndpointAsync(request.Module, request.Endpoint, args);
        }
        catch (Exception e)
        {
            Debug.Log("error catched \n" + e.Message);
            error = e.Message;
        }
        Debug.Log($"Completed Request {request.Module} - {request.Endpoint} - {(error == string.Empty ? "Success" : "Error")}");
        return new ModuleResponse(request, result, error);
    }

    public async Task<ModuleResponse<T>> Request<T>(PubnubRequest request)
    {
        Debug.Log($"Started Request {request.Module} - {request.Endpoint}");
        Dictionary<string, object> args = JObject.FromObject(request).ToObject<Dictionary<string, object>>();
        T result = default(T);
        string error = string.Empty;
        try
        {
            result = await Unity.Services.CloudCode.CloudCodeService.Instance.CallModuleEndpointAsync<T>(request.Module, request.Endpoint, args);
        }
        catch (Exception e)
        {
            Debug.Log("error catched");
            error = e.Message;
        }
        Debug.Log($"Completed Request {request.Module} - {request.Endpoint}");
        return new ModuleResponse<T>(request, result, error);
    }

    public async void Request<T>(PubnubRequest request, Action<T> callback, Action<string> error)
    {
        var response = await Request<T>(request);
        if (response.Result)
        {
            callback?.Invoke(response.Response);
        }
        else
        {
            error?.Invoke(response.Error);
        }
    }
}
