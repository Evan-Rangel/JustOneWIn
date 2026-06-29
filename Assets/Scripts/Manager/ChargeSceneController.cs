using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class ChargeSceneController : MonoBehaviour
    {
        [SerializeField] float timeToLoad;
        private void Start()
        {
            Invoke(nameof(LoadScene),timeToLoad);
        }

        void LoadScene()
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("SceneToLoad"));
        }
    }
}
