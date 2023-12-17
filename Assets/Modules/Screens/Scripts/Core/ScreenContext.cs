using System;

public class ScreenContext<T> : IScreenContext where T : IScreen
{
    public event Action ContextUpdated;

    protected void NotifyContentUpdate()
    {
        ContextUpdated?.Invoke();
    }
}

public interface IScreenContext
{
    event Action ContextUpdated;
}

