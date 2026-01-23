using UnityEngine;

namespace Avocado
{
    public class ResetPlayerPrefs : MonoBehaviour
    {
        void Start()
        {
           SaveManager.DeleteSaved();

        }
    }
}
