using System;
using UnityEngine;

namespace Avocado
{
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "ScriptableObjects/New Dialogue Data", order = 1)]
    public class Dialogue_SO : ScriptableObject
    {
        [field: SerializeField] public string dialogueKey { get; private set; }
        [field: SerializeField] public string[] dialogue { get; private set; }
    }
    [Serializable]
    public class DialogueInstance
    {
        public Dialogue_SO dialogueData { get; private set; }
        public bool saveDialogue{ get; private set; }
    }
}
