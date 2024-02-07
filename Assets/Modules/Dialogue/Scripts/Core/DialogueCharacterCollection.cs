using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterCollection : ScriptableObject
{
    public IReadOnlyList<DialogueCharacter> Characters => characters;
    [SerializeField] private List<DialogueCharacter> characters;


#if UNITY_EDITOR
    [Button("Fetch All Characters")]
    private void FetchCharacters()
    {
        characters = AssetsEditorUtility.GetAssets<DialogueCharacter>();
    }
#endif
}
