using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class NetworkMenu : MonoBehaviour
    {
        public void SinglePlayer()
        { 
            SceneManager.LoadScene("main");
        }
        public void Host()
        {
            NetworkManager.singleton.StartHost();
        }

        public void Join()
        {
            NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.StartClient();
        }
    }
}
