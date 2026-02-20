using System.Collections;
using System.Collections.Generic;
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
            dialogueTxt= GetComponentInChildren<TMP_Text>();
        }

        public void OnInputEnter()
        {
            CancelInvoke(nameof(WriteDialogue));
            if (currentIndex < currentText.Length - 1)
            {
                dialogueTxt.text = currentText;
                currentIndex = currentText.Length;
            }
            else
            {
                currentDialogueIndex++;
                dialogueTxt.text = "";

                if (currentDialogueIndex< currentDialogue.dialogue.Length)
                {
                    currentText = currentDialogue.dialogue[currentDialogueIndex];
                    currentIndex = 0;
                    InvokeRepeating(nameof(WriteDialogue), 0f, textSpeed);
                    return;
                }

                else
                {
                    currentDialogueIndex = 0;
                    currentIndex = 0;
                    currentDialogue = null;
                    gameObject.SetActive(false);
                    return;
                }

            }
        }

        public void StartDialogue(Dialogue_SO dialogue)
        {
            currentDialogue= dialogue;
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
