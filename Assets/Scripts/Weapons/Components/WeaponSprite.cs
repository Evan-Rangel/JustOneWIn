using System;
using System.Linq;
using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente (WeaponSprite) se encarga de mostrar el sprite correcto del arma durante una 
animación de ataque. Para ello:
Se sincroniza con el SpriteRenderer del personaje: Cada vez que cambia el sprite del personaje 
base (por ejemplo, al pasar de una animación a otra), el arma actualiza su sprite correspondiente.
Soporta diferentes fases de ataque: Cada fase tiene un conjunto de sprites distinto, permitiendo 
efectos visuales más elaborados durante combos o ataques especiales.
Se basa en eventos: Reacciona tanto a eventos de animación (OnEnterAttackPhase) como a eventos 
de cambio de sprite (RegisterSpriteChangeCallback), manteniendo sincronización perfecta entre 
el personaje y su arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    // Componente que maneja el sprite del arma, sincronizándolo con las fases del ataque.
    // Cambia el sprite mostrado durante cada fase del ataque según lo definido en los datos del arma.
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer baseSpriteRenderer;   
        private SpriteRenderer weaponSpriteRenderer; 

        private int currentWeaponSpriteIndex;        
        private Sprite[] currentPhaseSprites;        

        // Se llama al comenzar el ataque, reinicia el índice del sprite.
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentWeaponSpriteIndex = 0;
        }

        // Se llama cuando comienza una nueva fase de ataque.
        private void HandleEnterAttackPhase(AttackPhases phase)
        {
            currentWeaponSpriteIndex = 0;

            // Obtiene los sprites correspondientes a la fase actual
            currentPhaseSprites = currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;
        }

        // Se llama cuando cambia el sprite base. Se sincroniza el sprite del arma con el correspondiente en la fase.
        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            // Si no está en ataque, limpia el sprite del arma
            if (!isAttackActive)
            {
                weaponSpriteRenderer.sprite = null;
                return;
            }

            // Si el índice sobrepasa la cantidad de sprites disponibles
            if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
            {
                Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
                return;
            }

            // Asigna el siguiente sprite del arma y avanza el índice
            weaponSpriteRenderer.sprite = currentPhaseSprites[currentWeaponSpriteIndex];
            currentWeaponSpriteIndex++;
        }

        // Inicialización: obtiene componentes y registra callbacks.
        protected override void Start()
        {
            base.Start();

            // Obtiene los SpriteRenderers necesarios
            baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();

            // Obtiene los datos del arma relacionados al sprite
            data = weapon.Data.GetData<WeaponSpriteData>();

            // Registra el método que se llama cuando cambia el sprite base
            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);

            // Registra el método que se llama cuando entra a una fase de ataque
            AnimationEventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        // Limpieza: desuscribe eventos al destruirse el componente.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
            AnimationEventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
        }
    }
}
