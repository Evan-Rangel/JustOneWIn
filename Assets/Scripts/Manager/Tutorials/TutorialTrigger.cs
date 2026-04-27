using UnityEngine;
using System;
namespace Avocado
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] TutorialSO TutorialData;
        [SerializeField] KeyboardKeySO keyboardKey;
        event Action<TutorialSO> showTutorial;
         event Action<KeyboardKeySO, Vector2> shortTutorial;
        [SerializeField] bool isShortTutorial;
        private void Start()
        {
            if (SaveManager.IsTutorialKeySaved(TutorialData.name)) gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            showTutorial=TutorialManager.Instance.ShowTutorialItem;
            shortTutorial= PopOutTextManager.instance.ShowIcon;
        }
        private void OnDisable()
        {
            showTutorial -= TutorialManager.Instance.ShowTutorialItem;
            shortTutorial-= PopOutTextManager.instance.ShowIcon;
        }
        public void ShowTutorial()
        {
            showTutorial?.Invoke(TutorialData);
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (isShortTutorial)
                    shortTutorial?.Invoke (keyboardKey, (Vector2)transform.position+Vector2.up*5);
                else
                    ShowTutorial();
            }
        }
       /* private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ShowTutorial();
            }
        }*/
    }
}
