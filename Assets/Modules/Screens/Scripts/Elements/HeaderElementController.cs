using NaughtyAttributes;
using UnityEngine;

public class HeaderElementController: UIElementController<Header>
{
    [ShowIf("Option", ComponentDisplayOption.Show)]
    [SerializeField] private Sprite Icon;
    [ShowIf("Option", ComponentDisplayOption.Show)]
    [SerializeField] private string Title;

    public override void OnOpen(bool isNew)
    {
        signals.Fire(new ToggleHeaderSignal(Icon, Title, Option, screen));
    }
}

public class ToggleHeaderSignal : ToggleUIElementSignal
{
    public ToggleHeaderSignal(Sprite icon, string title, ComponentDisplayOption option, IScreen screen) : base(typeof(Header), option, screen)
    {
        Icon = icon;
        Title = title;
    }

    public Sprite Icon { get; protected set; }
    public string Title { get; protected set; }
}
