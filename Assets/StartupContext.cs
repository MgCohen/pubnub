public class StartupContext: ScreenContext<StartupScreen>
{
    public int Progress { get; private set; }

    public void SetProgress(int amount)
    {
        Progress = amount;
        NotifyContentUpdate();
    }
}
