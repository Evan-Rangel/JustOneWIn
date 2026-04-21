using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class NetworkMenu : MonoBehaviour
    {
        [SerializeField] string[] sceneNames;
        public void SinglePlayer()
        {
            if (SaveManager.GetSceneSpawnName() == "" || SaveManager.GetSceneSpawnName() == null)
            {
                SceneManager.LoadScene("RoomTutorial");
                return;
            }
           
            SceneManager.LoadScene(SaveManager.GetSceneSpawnName());
            return;
            
            //SceneManager.LoadScene("main");
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
