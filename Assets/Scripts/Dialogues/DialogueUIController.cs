using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Avocado
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] GameObject dialogueHolder;
        [SerializeField] TMP_Text dialogueTxt;
        Dialogue_SO currentDialogue;
        [SerializeField]int currentDialogueIndex;
        int currentIndex;
        [SerializeField] Image border;
        [SerializeField] float textSpeed = 0.05f;
        public static DialogueUIController instance;
        [SerializeField] Dialogue_SO testDialogueSo;
        UnityEvent onDialogueEventEnd;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            dialogueTxt= GetComponentInChildren<TMP_Text>();
            dialogueHolder.SetActive(false);
        }
        public void OnInputEnter()
        {
            if (!dialogueHolder.activeSelf)
                return;
            CancelInvoke(nameof(WriteDialogue));

            if (currentIndex < currentDialogue.dialogue[currentDialogueIndex].text.Length - 1)
            {
                dialogueTxt.text = currentDialogue.dialogue[currentDialogueIndex].text;
                currentIndex = currentDialogue.dialogue[currentDialogueIndex].text.Length;
                return;
            }

            currentDialogueIndex++;
            dialogueTxt.text = "";
            currentIndex = 0;
            
            if (currentDialogueIndex< currentDialogue.dialogue.Length)
            {
                border.color = currentDialogue.dialogue[currentDialogueIndex].borderColor;
                InvokeRepeating(nameof(WriteDialogue), 0f, textSpeed);
                return;
            }

            if (currentDialogue.saveDialogue)
                SaveManager.SaveDialogueKey(currentDialogue.dialogueKey);

            currentDialogueIndex = 0;
            currentDialogue = null;
            onDialogueEventEnd?.Invoke();
            dialogueHolder.SetActive(false);
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);
            return;
        }
        
        public void StartDialogue(Dialogue_SO _dialogue, UnityEvent _onDialogueEventEnd)
        {
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            Debug.Log("Starting Dialogue: " + _dialogue.dialogueKey);
            onDialogueEventEnd = _onDialogueEventEnd;

            dialogueHolder.SetActive(true);
            currentDialogueIndex = 0;
            currentDialogue= _dialogue;
            border.color = _dialogue.dialogue[currentDialogueIndex].borderColor;
            currentIndex = 0;
            currentDialogueIndex = 0;
            dialogueTxt.text = "";
            InvokeRepeating(nameof(WriteDialogue), 0f, textSpeed);
        }
        void WriteDialogue()
        { 
            dialogueTxt.text += currentDialogue.dialogue[currentDialogueIndex].text[currentIndex];
            currentIndex++;
            if (currentIndex < currentDialogue.dialogue[currentDialogueIndex].text.Length) return;
            CancelInvoke(nameof(WriteDialogue));
        }
    }
}
