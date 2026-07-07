using System;
using UnityEngine;

namespace Avocado
{
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "ScriptableObjects/New Dialogue Data", order = 1)]
    public class Dialogue_SO : ScriptableObject
    {
        [field: SerializeField] public string dialogueKey { get; private set; }
        [field: SerializeField] public DialogueData[] dialogue { get; private set; }
        [field: SerializeField] public bool saveDialogue { get; private set; }
        //[field:SerializeField]

    }
    [Serializable]
    public class DialogueData
    {
        public string text;
        public Color borderColor;
    }
    
}
