public interface IScreenService
{
    IScreen CurrentScreen { get; }

    void Close(IScreen screen);
    void Close<T>() where T : Screen;
    void CloseAll();
    void CloseCurrentScreen();
    T Open<T>(bool closeCurrent = false) where T : Screen;
    T Open<T>(ScreenContext<T> context, bool closeCurrent = false) where T : Screen;
}