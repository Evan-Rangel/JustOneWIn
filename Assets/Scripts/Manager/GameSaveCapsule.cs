using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool spawnWeaponAnimation;
        public bool closeCapsuleAnimation;
        int spawnWeaponDirection=1;
        public event Action OnSpawnWeaponAnimationEnd;
        public event Action OnCloseAnimationEnd;
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
