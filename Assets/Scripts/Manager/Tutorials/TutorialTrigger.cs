using UnityEngine;
using System;
namespace Avocado
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] TutorialSO TutorialData;
         event Action<TutorialSO> showTutorial;
        private void Start()
        {
            if (SaveManager.IsTutorialKeySaved(TutorialData.name)) gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            showTutorial=TutorialManager.Instance.ShowTutorialItem;
        }
        private void OnDisable()
        {
            showTutorial -= TutorialManager.Instance.ShowTutorialItem;
        }
        public void ShowTutorial()
        {
            showTutorial?.Invoke(TutorialData);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ShowTutorial();
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ShowTutorial();
            }
        }
    }
}
