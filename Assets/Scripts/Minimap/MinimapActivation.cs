using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class MinimapActivation : MonoBehaviour
    {
        [SerializeField] GameObject minimap;
        Transform minimapCamera;
        Transform player;
        public static MinimapActivation Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            minimapCamera = MinimapCamera.Instance.transform;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            minimap.SetActive(false);
        }

        private void Update()
        {
            if (!minimap.activeSelf) return;
       
            
            minimapCamera.position = new Vector3(player.position.x, player.position.y,0);
        }
        public void ActivateMinimap(bool _active)
        {
            minimap.SetActive(_active);
            MinimapCamera.Instance.gameObject.SetActive(_active);
            MinimapCamera.Instance.onMinimap = _active;
            MinimapCamera.Instance.SetFieldOfView(170);
        }
    }
}
