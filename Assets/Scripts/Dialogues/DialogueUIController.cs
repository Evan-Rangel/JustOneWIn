using UnityEngine;
using TMPro;
namespace Avocado
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] TMP_Text dialogueTxt;
        Dialogue_SO currentDialogue;
        int currentDialogueIndex;
        string currentText;
        int currentIndex;
        bool saveCurrentDialogue;
        [SerializeField] float textSpeed = 0.05f;

        public static DialogueUIController instance;
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
            saveCurrentDialogue = false;
            dialogueTxt= GetComponentInChildren<TMP_Text>();
        }

        public void OnInputEnter()
        {
            CancelInvoke(nameof(WriteDialogue));

            if (currentIndex < currentText.Length - 1)
            {
                dialogueTxt.text = currentText;
                currentIndex = currentText.Length;
                return;
            }

            if (saveCurrentDialogue)
                SaveManager.SaveDialogueKey(currentDialogue.dialogueKey);

            currentDialogueIndex++;
            dialogueTxt.text = "";
            currentIndex = 0;
            
            if (currentDialogueIndex< currentDialogue.dialogue.Length)
            {
                currentText = currentDialogue.dialogue[currentDialogueIndex];
                InvokeRepeating(nameof(WriteDialogue), 0f, textSpeed);
                return;
            }

            currentDialogueIndex = 0;
            currentDialogue = null;
            gameObject.SetActive(false);
            return;
        }

        public void StartDialogue(DialogueInstance dialogue)
        {
            saveCurrentDialogue = dialogue.saveDialogue;
            currentDialogue= dialogue.dialogueData;
            currentIndex = 0;
            currentDialogueIndex = 0;
            currentText = currentDialogue.dialogue[0];
            dialogueTxt.text = "";
            InvokeRepeating(nameof(WriteDialogue), 0f, textSpeed);
        }
        void WriteDialogue()
        { 
            dialogueTxt.text += currentText[currentIndex];
            currentIndex++;
            if (currentIndex >= currentText.Length )
                CancelInvoke(nameof(WriteDialogue));
        }
        private void OnDisable()
        {
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);
        }
        private void OnEnable()
        {
            GameManager.instance.ChangeState(GameManager.GameState.UI);
        }
    }
}
