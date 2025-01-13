using UnityEngine;
[CreateAssetMenu(fileName = "CharacterUI", menuName = "ScriptableObjects/Character")]

public class CharacterData : ScriptableObject
{
    [field: SerializeField] public Sprite[] skins { get; private set; }
    [field: SerializeField] public string cName { get; private set; }
    [field: SerializeField] public Color color { get; private set; }
}
