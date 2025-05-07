using Avocado.Combat.Damage;
using UnityEngine;
using static Avocado.Utilities.CombatDamageUtilities; // Importación estática para llamar a funciones de utilidad directamente

/*---------------------------------------------------------------------------------------------
DamageOnParry es un componente que permite aplicar daño automáticamente a un enemigo cuando el 
jugador realiza un parry exitoso. Para lograr esto, se suscribe al evento OnParry de otro 
componente llamado Parry.
Cuando ocurre el evento, llama a la utilidad TryDamage, que maneja la verificación de si el 
enemigo puede recibir daño (IDamageable) y aplica el daño correspondiente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DamageOnParry : WeaponComponent<DamageOnParryData, AttackDamage>
    {
        private Parry parry; // Referencia al componente de Parry en el mismo objeto

        private void HandleParry(GameObject parriedGameObject)
        {
            // Aplica daño usando la utilidad TryDamage
            TryDamage(parriedGameObject, new DamageData(currentAttackData.Amount, Core.Root), out _);
        }

        // Inicializa el componente, obtiene el Parry y se suscribe al evento.
        protected override void Start()
        {
            base.Start();

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
