using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Avocado
{
    public class RoomTransitionController : MonoBehaviour
    {
        [field: SerializeField] public string nextSceneName { get; private set; }
        [field: SerializeField] public string nextSceneDoorName { get; private set; }


        public void InitTransition()
        {
            PlayerPrefs.SetString("DoorName", nextSceneDoorName);
            FakeLight_S.instance.ShadeEffect(()=>SceneManager.LoadScene(nextSceneName));
            //StartCoroutine(DelayTransition());
        }
        IEnumerator DelayTransition()
        {
            yield return Helpers.GetWait(.5f);
            SceneManager.LoadScene(nextSceneName);

        }
    }
}
