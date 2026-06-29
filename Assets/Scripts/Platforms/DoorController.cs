using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] string eventName;
        Animator animator;
        FabricEnemyCollision activateCollision;
        event Action<string, Transform> OnPlaySound;
        [SerializeField] bool startActivated;
        [SerializeField] bool activatedOnTrigger;
        [SerializeField] bool onSaveIsActive;
        public UnityEvent onTriggerIsActivated, onTriggerIsDeactivated;
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
            {
                if (activateCollision!=null)
                    activateCollision.gameObject.SetActive(false);
                if (onSaveIsActive)
                    ActivateBlock();
                else
                    DeactivateBlock();
                return;
            }
            
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
            if (SaveManager.IsEventKeySaved(eventName))return;
            activateCollision.gameObject.SetActive(false);
            if (activatedOnTrigger) onTriggerIsActivated?.Invoke();
            else onTriggerIsDeactivated?.Invoke();
        }
        public void ActivateBlockAndSaveEvent()
        {
            ActivateBlock();
            if (eventName == "" || eventName == string.Empty || eventName == null)
                return;
            SaveManager.SaveEventKey(eventName);
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
