using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente muestra un sprite adicional opcional durante un ataque, como por ejemplo un 
efecto visual o una animación especial. Está pensado para usarse solo en ataques que lo necesiten.
-El sprite se coloca en pantalla mediante animaciones, y su visibilidad se controla con eventos 
de animación (OnSetOptionalSpriteActive).
-Usa un marcador (OptionalSpriteMarker) para encontrar el SpriteRenderer adecuado dentro del objeto.
-Si el ataque actual lo permite (UseOptionalSprite), se le asigna el sprite definido en los datos 
del ataque.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class OptionalSprite : WeaponComponent<OptionalSpriteData, AttackOptionalSprite>
    {
        private SpriteRenderer spriteRenderer; // Referencia al sprite renderer del sprite opcional

        // Evento que activa o desactiva el sprite opcional. Llamado desde animaciones por medio de AnimationEventHandler.
        private void HandleSetOptionalSpriteActive(bool value)
        {
            spriteRenderer.enabled = value;
        }

        // Lógica que se ejecuta al iniciar un ataque. Asigna el sprite si está habilitado para el ataque actual.
        protected override void HandleEnter()
        {
            base.HandleEnter();

            if (!currentAttackData.UseOptionalSprite)
                return;

            spriteRenderer.sprite = currentAttackData.Sprite;
        }

        // Obtiene la referencia al SpriteRenderer desde un hijo marcado con OptionalSpriteMarker.
        protected override void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponentInChildren<OptionalSpriteMarker>().SpriteRenderer;
            spriteRenderer.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            AnimationEventHandler.OnSetOptionalSpriteActive += HandleSetOptionalSpriteActive;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            AnimationEventHandler.OnSetOptionalSpriteActive -= HandleSetOptionalSpriteActive;
        }
    }
}
