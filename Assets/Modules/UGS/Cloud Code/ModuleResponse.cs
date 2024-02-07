public class ModuleResponse<T>
{
    public ModuleResponse(ModuleRequest request, T response, string error = null)
    {
        Request = request;
        Result = string.IsNullOrEmpty(error);
        Response = response;
    }

    public ModuleRequest Request { get; private set; }
    public bool Result { get; private set; }
    public string Error { get; private set; }
    public T Response { get; private set; }
}


public class ModuleResponse : ModuleResponse<string>
{
    public ModuleResponse(ModuleRequest request, string response, string error = null) : base(request, response, error)
    {
    }
}