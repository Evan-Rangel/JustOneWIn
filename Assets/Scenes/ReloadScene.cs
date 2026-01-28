using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class ReloadScene : MonoBehaviour
    {
        private void Start()
        {
            Invoke("LoadScene", 0.1f);
        }
        void LoadScene()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
