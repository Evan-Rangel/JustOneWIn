using UnityEngine;

namespace Avocado
{
    public class GameSave : InteractEventManager
    {
        [SerializeField] Vector3 savePointPosition;
        public override void OnInteractEvent()
        {
            base.OnInteractEvent();

        }
    }
}
