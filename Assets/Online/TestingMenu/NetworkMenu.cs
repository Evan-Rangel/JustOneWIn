using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class NetworkMenu : MonoBehaviour
    {
        public void SinglePlayer()
        {
            if (SaveManager.GetSceneSpawnName() == "" || SaveManager.GetSceneSpawnName() == null)
                PlayerPrefs.SetString("SceneToLoad", "RoomTutorial");
            else
                PlayerPrefs.SetString("SceneToLoad", SaveManager.GetSceneSpawnName());
            SceneManager.LoadScene("RoomAssetCharge");
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
