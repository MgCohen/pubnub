
using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="Game/Screens/Dictionary")]
public class ScreenDictionary: ScriptableObject
{
    [SerializeField] private List<ScreenReference> screens = new List<ScreenReference>();
    
    public bool TryGetScreen(Type screenType, out AssetReference screenAsset)
    {
        foreach(var screen in screens)
        {
            if (screen.TryMatch(screenType))
            {
                screenAsset = screen.Asset;
                return true;
            }
        }

        screenAsset = null;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach(var screenRef in screens)
        {
            if(screenRef.Asset != null)
            {
                GameObject obj = screenRef.Asset.editorAsset as GameObject;
                IScreen screen = obj.GetComponent<IScreen>();
                screenRef.SetType(screen.GetType());
            }
        }
    }
#endif
    [Serializable]
    public class ScreenReference
    {
        public AssetReference Asset => asset;
        [SerializeField] private AssetReference asset;
        public Type Type => type.Type;
        [SerializeField, NaughtyAttributes.ReadOnly] private TypeReference type;

        public bool TryMatch<T>()
        {
            return type.Type == typeof(T);
        }

        public bool TryMatch(Type match)
        {
            return type.Type == match;
        }

        public void SetType(Type type)
        {
            this.type = type;
        }
    }
}


