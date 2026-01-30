using System;
using UnityEngine;

namespace Avocado
{
    [CreateAssetMenu(fileName = "TutorialSO", menuName = "ScriptableObjects/TutorialSO", order = 1)]
    public class TutorialSO : ScriptableObject
    {
        public KeyboardKeySO keyboardKey;
        public Sprite[] iconSprite;
        public string description;
    }

}
