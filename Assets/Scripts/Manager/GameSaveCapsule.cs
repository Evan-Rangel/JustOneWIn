using System;
using UnityEngine;

namespace Avocado
{
    public class GameSaveCapsule : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        [SerializeField]Sprite[] sprites;
        bool isOpen = false;
        int currentIndex;
        [SerializeField]float maxTimer;  
        float timer;
       bool spawnWeaponAnimation;
        bool closeCapsuleAnimation;
        int spawnWeaponDirection=1;
        public event Action OnSpawnWeaponAnimationEnd;
        public event Action OnCloseAnimationEnd;
        event Action<string, Transform> OnPlaySound;

        private void OnEnable()
        {
            OnPlaySound += AudioManager.instance.PlaySFXSound;
        }
        private void OnDisable()
        {
            OnPlaySound -= AudioManager.instance.PlaySFXSound;
        }
        public void StartCloseCapsuleAnimation()
        {
            OnPlaySound?.Invoke("CapsuleOpen", transform);
            closeCapsuleAnimation = true;
        }
        public void StartWeaponAnimation()
        { 
            OnPlaySound?.Invoke("CapsuleOpen", transform);
            spawnWeaponAnimation = true;
        }

        private void Update()
        {
            timer -= Time.deltaTime;    
            if (timer > 0)
                return;
            timer = maxTimer;
            if (closeCapsuleAnimation)
            {
                if (currentIndex >= sprites.Length - 1)
                {
                    spawnWeaponDirection = -1;
                }

                currentIndex += ((currentIndex == 0 && spawnWeaponDirection == -1) ||
                    (currentIndex == sprites.Length && spawnWeaponDirection == 1))
                    ? 0 : spawnWeaponDirection;

                if (currentIndex == 0)
                {
                    OnPlaySound?.Invoke("CapsuleClose", transform);

                    spawnWeaponDirection = 1;
                    closeCapsuleAnimation = false;
                    OnCloseAnimationEnd?.Invoke();
                    OnCloseAnimationEnd = null;
                }

                spriteRenderer.sprite = sprites[currentIndex];
                return;
            }

            if (spawnWeaponAnimation)
            {
                if (currentIndex >= sprites.Length - 1)
                {
                    spawnWeaponDirection = -1;
                } 

                currentIndex += ((currentIndex == 0 && spawnWeaponDirection==-1) || 
                    (currentIndex == sprites.Length && spawnWeaponDirection==1)) 
                    ? 0 : spawnWeaponDirection;

                if (currentIndex == 0)
                {
                    OnPlaySound?.Invoke("CapsuleClose", transform);
                    spawnWeaponAnimation = false;
                    spawnWeaponDirection = 1;
                    OnSpawnWeaponAnimationEnd?.Invoke();
                    OnSpawnWeaponAnimationEnd = null;
                }

                spriteRenderer.sprite = sprites[currentIndex];
                return;
            }
            
            
            
            
            int direction = isOpen ? 1 : -1;
            currentIndex +=((currentIndex==0&&!isOpen) || (currentIndex==sprites.Length&&isOpen))?0: direction;
            
            if ((!isOpen && currentIndex == 0) || currentIndex >= sprites.Length)
                return;
            spriteRenderer.sprite = sprites[currentIndex];
        }


        private void Start()
        {
            spriteRenderer=GetComponent<SpriteRenderer>();
            spriteRenderer.sprite= sprites[0];
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnPlaySound?.Invoke("CapsuleOpen", transform);
                isOpen = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isOpen = false;
            }
        }
    }
}
