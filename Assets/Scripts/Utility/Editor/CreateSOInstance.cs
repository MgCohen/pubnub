using UnityEditor;
using UnityEngine;

public class CreateSOInstance
{
    [MenuItem("Assets/Create Scriptable Object from selection")]
    static void Create()
    {
        // Get the selected asset in the Project view
        Object selectedAsset = Selection.activeObject;

        // Check if it's a script
        if (selectedAsset != null && selectedAsset.GetType() == typeof(MonoScript))
        {
            // Get the type of the selected script
            System.Type selectedType = ((MonoScript)selectedAsset).GetClass();

            // Check if it's a ScriptableObject
            if (selectedType != null && selectedType.IsSubclassOf(typeof(ScriptableObject)))
            {
                // Create a new instance of the selected type
                ScriptableObject newObj = ScriptableObject.CreateInstance(selectedType);

                // Create a new asset file and save the new ScriptableObject to it
                string assetPath = AssetDatabase.GetAssetPath(selectedAsset);
                string newAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath.Replace(".cs", ".asset"));
                AssetDatabase.CreateAsset(newObj, newAssetPath);
                AssetDatabase.SaveAssets();

                // Select the newly created asset in the Project view
                Selection.activeObject = newObj;
            }
        }
    }
}