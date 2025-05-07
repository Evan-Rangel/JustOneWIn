using UnityEngine;
using static Avocado.Combat.KnockBack.CombatKnockBackUtilities; // Permite usar TryKnockBack directamente

/*---------------------------------------------------------------------------------------------
Este componente se activa cuando el jugador realiza una parada (parry) exitosa. Al detectar que 
un objeto ha sido parado, el script intenta aplicar un efecto de retroceso (knockback) a ese 
objeto, usando la utilidad TryKnockBack.
Esto puede servir para efectos como empujar a un enemigo hacia atrás cuando bloqueas su ataque 
en el momento justo.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class KnockBackOnParry : WeaponComponent<KnockBackOnParryData, AttackKnockBack>
    {
        private Parry parry; // Referencia al componente de Parry
        private CoreSystem.Movement movement; // Acceso a la dirección del personaje

        // Cuando ocurre una parry, intenta aplicar knockback al objeto parado.
        private void HandleParry(GameObject parriedGameObject)
        {
            TryKnockBack(parriedGameObject, new Combat.KnockBack.KnockBackData(currentAttackData.Angle, currentAttackData.Strength, movement.FacingDirection, Core.Root), out _);
        }

        protected override void Start()
        {
            base.Start();

            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            parry = GetComponent<Parry>();

            parry.OnParry += HandleParry;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            parry.OnParry -= HandleParry;
        }
    }
}
