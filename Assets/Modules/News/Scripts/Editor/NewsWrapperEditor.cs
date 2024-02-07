using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(NewsWrapper))]
[CanEditMultipleObjects]
public class NewsWrapperEditor : Editor
{
    public Dictionary<string, SerializedProperty> assetProps = new Dictionary<string, SerializedProperty>();

    public override void OnInspectorGUI()
    {
        var newsProp = serializedObject.FindProperty("news");
        foreach (var childProp in newsProp.GetDirectChildren())
        {
            if (childProp.name.Contains("GUID"))
            {
                DrawAssetField(childProp);
            }
            else
            {
                EditorGUILayout.PropertyField(childProp);
            }
        }
    }

    private void DrawAssetField(SerializedProperty assetProp)
    {
        var address = assetProp.stringValue;
        var prop = GetAssetProp(assetProp.name, address);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(prop, new GUIContent(assetProp.name));
        if (EditorGUI.EndChangeCheck())
        {
            var entry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(prop.FindPropertyRelative("m_AssetGUID").stringValue);
            assetProp.stringValue = entry.address;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }

    private SerializedProperty GetAssetProp(string propName, string assetAddress)
    {
        if(assetProps.TryGetValue(propName, out SerializedProperty prop) && prop != null)
        {
            return prop;
        }

        var assets = new List<AddressableAssetEntry>();
        AddressableAssetSettingsDefaultObject.Settings.GetAllAssets(assets, false, entryFilter: (e) => { return e.address == assetAddress; });
        var assetGUID = assets.FirstOrDefault()?.guid ?? string.Empty;

        var soInstance = ScriptableObject.CreateInstance<AssetReferenceWrapper>();
        soInstance.Asset = new UnityEngine.AddressableAssets.AssetReference(assetGUID);
        var serializedObject = new SerializedObject(soInstance);
        var property = serializedObject.FindProperty("Asset");
        assetProps[propName] = property;
        return property;
    }
}
