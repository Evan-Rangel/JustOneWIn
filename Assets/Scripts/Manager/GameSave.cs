using UnityEngine;

namespace Avocado
{
    public class GameSave : InteractEventManager
    {
        [SerializeField] Transform savePointPosition;
        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            SaveManager.SavePlayerPositionInPlayerPrefs(savePointPosition.position);
        }
    }
}
