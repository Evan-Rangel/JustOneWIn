using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
    public class DialogueEventActivation : MonoBehaviour
    {
        [SerializeField] Dialogue_SO dialogue;
        [SerializeField] UnityEvent onDialogueEnd;
        [SerializeField] bool activateOnStart;
        private void Start()
        {
            StartDialogue();
            if (activateOnStart)
            {
            //    Invoke(nameof(StartDialogue), 1.5f);
            }
        }
        public void StartDialogue()
        {
            DialogueUIController.instance.StartDialogue(dialogue, onDialogueEnd);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                DialogueUIController.instance.StartDialogue(dialogue, onDialogueEnd);
               // PopOutTextManager.instance.ShowText("Press E to talk", transform.position + Vector3.up * 1.5f);
            }
        }
    }   
}
