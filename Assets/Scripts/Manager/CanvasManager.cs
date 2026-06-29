using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        [Header("Minimap")]
        [SerializeField] GameObject minimapHolder;
        public void ToggleMinimap(bool _active)
        {
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            minimapHolder.SetActive(_active);
        }
    }
}
