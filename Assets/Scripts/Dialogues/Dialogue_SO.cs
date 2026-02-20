using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{


    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "ScriptableObjects/New Dialogue Data", order = 1)]

    public class Dialogue_SO : ScriptableObject
    {
        [field: SerializeField] public string dialogueKey { get; private set; }
        [field: SerializeField] public string[] dialogue { get; private set; }
        
    }
}
