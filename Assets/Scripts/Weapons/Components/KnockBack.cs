using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente KnockBack aplica un efecto de retroceso a los enemigos u objetos que detecte 
dentro del ActionHitBox. Cuando el HitBox detecta colisiones, se revisa si esos objetos pueden 
recibir retroceso (IKnockBackable). Si es así, se les aplica una fuerza en determinada dirección 
y ángulo.
Este efecto es útil, por ejemplo, en ataques cuerpo a cuerpo que hacen que el enemigo salga 
volando o se mueva hacia atrás.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class KnockBack : WeaponComponent<KnockBackData, AttackKnockBack>
    {
        private ActionHitBox hitBox; // Componente que detecta colisiones con otros objetos
        private CoreSystem.Movement movement; // Componente que contiene información sobre la dirección del personaje

        // Maneja lo que sucede cuando el hitbox detecta uno o más colliders.
        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IKnockBackable knockBackable))
                {
                    knockBackable.KnockBack(new Combat.KnockBack.KnockBackData(currentAttackData.Angle, currentAttackData.Strength, movement.FacingDirection, Core.Root));
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;

            movement = Core.GetCoreComponent<CoreSystem.Movement>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}
