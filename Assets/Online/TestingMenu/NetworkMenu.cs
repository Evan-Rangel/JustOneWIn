using Mirror;
using UnityEngine;

namespace Avocado
{
    public class NetworkMenu : MonoBehaviour
    {
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
