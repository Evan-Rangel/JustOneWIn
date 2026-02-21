using UnityEngine;

namespace Avocado
{
    public class DialogueEventActivation : MonoBehaviour
    {
        [SerializeField] DialogueInstance[] dialogue;
        int currentDialogueIndex;

        private void Awake()
        {
            currentDialogueIndex = 0;
        }
        public void StartDialogueInIndex(int idx)
        {
            currentDialogueIndex = idx;
            GameManager.instance.ActiveDialogueWindow();
            DialogueUIController.instance.StartDialogue(dialogue[idx]);
        }
        public void RequestDialogue()
        {
            if (currentDialogueIndex == dialogue.Length)
                return;
            if (SaveManager.IsDialogueKeySaved(dialogue[currentDialogueIndex].dialogueData.dialogueKey))
            {
                currentDialogueIndex++;
                RequestDialogue();
                return;
            }
            StartDialogueInIndex(currentDialogueIndex);
        }
    }   
}
