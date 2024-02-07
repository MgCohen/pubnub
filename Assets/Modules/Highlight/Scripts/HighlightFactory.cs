using UnityEngine;
using Zenject;

public class HighlightFactory : IFactory<GameObject, Highlight.PointerOptions, Highlight>
{
    public HighlightFactory(DiContainer container)
    {
        this.container = container;
    }
    
    private DiContainer container;
    
    public Highlight Create(GameObject target, Highlight.PointerOptions pointerOptions)
    {
        return container.Instantiate<LayerHighlight>(new object[] { target, pointerOptions });
    }
}
