using System;
using Avocado.CoreSystem;
using Avocado.Utilities;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente Charge se encarga de acumular cargas (como energía o poder) a lo largo del 
tiempo mientras un ataque está activo. Usa un temporizador (TimeNotifier) para incrementar el 
número de cargas en intervalos regulares.
Cuando se alcanza el número máximo de cargas, el temporizador se desactiva y se generan 
partículas que indican una carga completa. Mientras tanto, cada incremento también lanza 
partículas para mostrar visualmente el progreso. Cuando el ataque termina o se interrumpe, 
el sistema detiene el conteo de cargas.
Este tipo de sistema es ideal para mecánicas como ataques que se cargan más fuerte mientras 
más tiempo se mantenga presionado un botón.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class Charge : WeaponComponent<ChargeData, AttackCharge>
    {
        private int currentCharge; // Almacena la cantidad actual de cargas

        // Temporizador que lanza un evento en intervalos definidos (según el tiempo de carga)
        private TimeNotifier timeNotifier;

        // Maneja la reproducción de efectos de partículas
        private ParticleManager particleManager;

        // Detiene el temporizador y devuelve la cantidad final de cargas acumuladas.
        public int TakeFinalChargeReading()
        {
            timeNotifier.Disable();
            return currentCharge;
        }

        // Cuando comienza el ataque, resetea las cargas y activa el temporizador.
        protected override void HandleEnter()
        {
            base.HandleEnter();

            // Carga inicial configurada en los datos del ataque
            currentCharge = currentAttackData.InitialChargeAmount;

            // Inicia el temporizador con el tiempo entre cargas
            timeNotifier.Init(currentAttackData.ChargeTime, true);
        }

        // Llamado cada vez que el temporizador se activa. Incrementa la carga.
        private void HandleNotify()
        {
            currentCharge++;

            // Si alcanzamos el máximo de cargas
            if (currentCharge >= currentAttackData.NumberOfCharges)
            {
                currentCharge = currentAttackData.NumberOfCharges;

                // Detiene el temporizador
                timeNotifier.Disable();

                // Partículas indicando que se alcanzó la carga máxima
                particleManager.StartParticlesRelative(
                    currentAttackData.FullyChargedIndicatorParticlePrefab,
                    currentAttackData.ParticlesOffset,
                    Quaternion.identity
                );
            }
            else
            {
                // Partículas indicando incremento de carga
                particleManager.StartParticlesRelative(
                    currentAttackData.ChargeIncreaseIndicatorParticlePrefab,
                    currentAttackData.ParticlesOffset,
                    Quaternion.identity
                );
            }
        }

        protected override void HandleExit()
        {
            base.HandleExit();

            timeNotifier.Disable();
        }

        // Se llama al inicializar. Crea el temporizador y se suscribe al evento de notificación.
        protected override void Awake()
        {
            base.Awake();

            timeNotifier = new TimeNotifier();
            timeNotifier.OnNotify += HandleNotify;
        }

        protected override void Start()
        {
            base.Start();

            particleManager = Core.GetCoreComponent<ParticleManager>();
        }

        private void Update()
        {
            timeNotifier.Tick(); // Muy importante: sin esto el temporizador no funcionará
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            timeNotifier.OnNotify -= HandleNotify;
        }
    }
}
