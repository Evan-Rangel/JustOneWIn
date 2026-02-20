using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class DialogueEventActivation : MonoBehaviour
    {
        public event Action<Dialogue_SO> onDialogueStart;
        [SerializeField] Dialogue_SO dialogue;

        private void Start()
        {
            Invoke(nameof(ActivateDialogueEvent), 3);
        }

       /* private void OnEnable()
        {
            onDialogueStart +=GameManager.instance.ActiveDialogue;
        }
        private void OnDisable()
        {
            onDialogueStart -= GameManager.instance.ActiveDialogue;
        }*/
        public void ActivateDialogueEvent()
        {
            GameManager.instance.ActiveDialogue(dialogue);
            //onDialogueStart?.Invoke(dialogue);
        }
    }
}
