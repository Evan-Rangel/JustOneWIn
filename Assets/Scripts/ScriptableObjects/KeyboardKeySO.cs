using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    [CreateAssetMenu(fileName = "New Keyboard Key", menuName = "ScriptableObjects/Keyboard Key")]
    public class KeyboardKeySO : ScriptableObject
    {
        [field:SerializeField] public string keyCode { get; private set; }
        [field:SerializeField] public Sprite OnKeySprite { get; private set; }
        [field: SerializeField] public Sprite OffKeySprite { get; private set; }
    }
}
