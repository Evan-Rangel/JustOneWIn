using UnityEngine;

namespace Avocado
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] TutorialSO tutorial;
        public void ShowAbilityTutorial()
        {
           TutorialManager.Instance.ShowTutorialItem(tutorial);
            Destroy(gameObject);

        }
    }
}
