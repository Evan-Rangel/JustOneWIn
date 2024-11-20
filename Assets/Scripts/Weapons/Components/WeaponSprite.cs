using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        #region References
        #endregion

        #region Components
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        #endregion

        #region Integers
        private int currentWeaponSpriteIndex;
        public void SetCurrentWeaponSpriteIndex(int idx) { currentWeaponSpriteIndex = idx; }
        #endregion

        private Sprite[] currentPhaseSprites;

        #region Functions
        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentWeaponSpriteIndex = 0;
        }

        private void HandleEnterAttackPhase(AttackPhases phase)
        {

            currentWeaponSpriteIndex = 0;
//            if (transform.root.name == "LocalGamePlayer")

                currentPhaseSprites = currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;
        }

        private void HandlerBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                weaponSpriteRenderer.sprite = null;
                return;
            }

            if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
            {
                Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
                return;
            }

            weaponSpriteRenderer.sprite = currentPhaseSprites[currentWeaponSpriteIndex];

            currentWeaponSpriteIndex++;
            GameManager.instance.ChangeWeaponSprite(currentWeaponSpriteIndex);
        }

        protected override void Start()
        {
            base.Start();

            baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();

            data = weapon.Data.GetData<WeaponSpriteData>();

            baseSpriteRenderer.RegisterSpriteChangeCallback(HandlerBaseSpriteChange);
//            if (transform.root.name== "LocalGamePlayer")

                eventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandlerBaseSpriteChange);
          //  if (transform.root.name == "LocalGamePlayer")

                eventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
        }
        #endregion
    }
}
