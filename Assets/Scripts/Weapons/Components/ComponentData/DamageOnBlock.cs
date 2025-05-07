using Avocado.Combat.Damage;                     
using UnityEngine;
using static Avocado.Utilities.CombatDamageUtilities;

/*---------------------------------------------------------------------------------------------
Este componente DamageOnBlock:
Hereda de WeaponComponent<DamageOnBlockData, AttackDamage>, por lo que usa datos de tipo AttackDamage.
Se activa automáticamente cuando esta arma bloquea un ataque, gracias al evento OnBlock del componente Block.
Llama a TryDamage, que intenta aplicar daño al oponente que fue bloqueado, usando la cantidad de daño definida en currentAttackData.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DamageOnBlock : WeaponComponent<DamageOnBlockData, AttackDamage>
    {
        // Referencia al componente de bloqueo de esta arma.
        private Block block;

        // Método que se llama cuando el arma bloquea algo.
        private void HandleBlock(GameObject blockedGameObject)
        {
            // Intenta aplicar daño al GameObject bloqueado.
            TryDamage(blockedGameObject, new DamageData(currentAttackData.Amount, Core.Root), out _);
        }

        // Suscribe el método de bloqueo al evento del componente Block.
        protected override void Start()
        {
            base.Start();

            block = GetComponent<Block>(); // Busca el componente de bloqueo en el mismo GameObject.

            block.OnBlock += HandleBlock; // Se suscribe al evento de bloqueo.
        }

        // Se asegura de desuscribirse del evento al destruirse el objeto.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            block.OnBlock -= HandleBlock; // Limpieza de evento para evitar errores o fugas.
        }
    }
}
