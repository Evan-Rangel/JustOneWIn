using UnityEngine;
[CreateAssetMenu(fileName = "CharacterUI", menuName = "ScriptableObjects/Character")]

public class CharacterData : ScriptableObject
{
    [field: SerializeField] public Sprite[] skins { get; private set; }
    [field: SerializeField] public string cName { get; private set; }
    [field: SerializeField] public AllCharacters cCharacter { get; private set; }
}
