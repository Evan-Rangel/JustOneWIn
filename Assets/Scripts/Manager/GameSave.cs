using UnityEngine;

namespace Avocado
{
    public class GameSave : InteractEventManager
    {
        [SerializeField] GameObject minimapPoint;
        [SerializeField] Transform savePointPosition;
        public override void Start()
        {
            base.Start();
            minimapPoint.SetActive(true);
        }
        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            // SaveManager.SavePlayerPositionInPlayerPrefs(savePointPosition.position);
        }
    }
}
