using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] string eventName;
        Animator animator;
        FabricEnemyCollision activateCollision;
        event Action<string, Transform> OnPlaySound;
        [SerializeField] bool startActivated;
        //public UnityEvent 
        private void Awake()
        {
            animator = GetComponent<Animator>();
            try
            {
                activateCollision = GetComponentInChildren<FabricEnemyCollision>();
            }
            catch { }
        }
        private void Start()
        {
            if (eventName == "" || eventName == string.Empty || eventName == null)
                return;
            if (SaveManager.IsEventKeySaved(eventName))
                DeactivateBlock();
            
            if (startActivated)
                ActivateBlock();
        }
        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;

            if (activateCollision != null)
                activateCollision.OnPlayerEnter += PlayerCollisionEnter;
        }
        private void OnDisable()
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;

            if (activateCollision != null)
                activateCollision.OnPlayerEnter -= PlayerCollisionEnter;
        }
        void PlayerCollisionEnter(Collider2D _player)
        {
            ActivateBlock();
        }

        public void ActivateBlock()
        {
            animator.SetTrigger("Activate");
        }
        public void DeactivateBlockAndSaveEvent()
        {
            DeactivateBlock();
            if (eventName == "" || eventName == string.Empty || eventName == null)
                return;
            SaveManager.SaveEventKey(eventName);
        }
        public void DeactivateBlock()
        {
            animator.SetTrigger("Deactivate");
        }
    }
}
