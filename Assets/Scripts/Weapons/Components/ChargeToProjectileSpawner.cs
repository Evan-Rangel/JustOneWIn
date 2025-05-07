using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente conecta el sistema de carga (Charge) con el sistema de disparo (ProjectileSpawner). 
Funciona así:
-Cuando comienza un ataque (HandleEnter), se reinicia el estado para permitir una nueva lectura 
de carga.
-Cuando el jugador suelta el botón de ataque (HandleCurrentInputChange con newInput == false), 
el componente:
--Lee cuántas cargas acumuló el jugador.
--Configura una estrategia personalizada que ajusta el comportamiento de disparo (por ejemplo, 
disparar más proyectiles o con mayor ángulo).
--Le asigna esa estrategia al ProjectileSpawner.
--Todo esto utiliza el patrón Strategy, que permite cambiar dinámicamente la lógica de disparo 
sin modificar el código del lanzador de proyectiles.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ChargeToProjectileSpawner : WeaponComponent<ChargeToProjectileSpawnerData, AttackChargeToProjectileSpawner>
    {
        private ProjectileSpawner projectileSpawner; 
        private Charge charge; // Referencia al componente que acumula carga

        private bool hasReadCharge; // Asegura que solo se lea una vez la carga durante el ataque

        // Estrategia que define cómo se lanzan los proyectiles dependiendo de la carga
        private ChargeProjectileSpawnerStrategy chargeProjectileSpawnerStrategy = new ChargeProjectileSpawnerStrategy();

        // Cuando comienza el ataque, resetea el estado de lectura de carga.
        protected override void HandleEnter()
        {
            base.HandleEnter();
            hasReadCharge = false;
        }

        // Se ejecuta cuando cambia el estado del input (cuando se suelta el botón de ataque).
        private void HandleCurrentInputChange(bool newInput)
        {
            // Si el botón aún está presionado o ya se leyó la carga, no hacemos nada
            if (newInput || hasReadCharge)
                return;

            // Configura la estrategia con los datos actuales
            chargeProjectileSpawnerStrategy.AngleVariation = currentAttackData.AngleVariation;
            chargeProjectileSpawnerStrategy.ChargeAmount = charge.TakeFinalChargeReading();

            // Aplica la nueva estrategia al lanzador de proyectiles
            projectileSpawner.SetProjectileSpawnerStrategy(chargeProjectileSpawnerStrategy);

            // Marca que ya leímos la carga para no repetir
            hasReadCharge = true;
        }

        protected override void Start()
        {
            base.Start();

            projectileSpawner = GetComponent<ProjectileSpawner>();
            charge = GetComponent<Charge>();

            weapon.OnCurrentInputChange += HandleCurrentInputChange;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            weapon.OnCurrentInputChange -= HandleCurrentInputChange;
        }
    }
}
