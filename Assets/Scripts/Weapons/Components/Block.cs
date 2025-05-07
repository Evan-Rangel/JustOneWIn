using System;
using Avocado.CoreSystem;
using Avocado.Utilities;
using Avocado.Weapons.Modifiers;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El script Block permite que un personaje bloquee ataques enemigos durante ventanas de tiempo 
específicas (block windows) definidas en los datos del ataque. Lo hace aplicando tres modificadores:
-DamageModifier para reducir o anular el daño.
-BlockKnockBackModifier para evitar el retroceso.
-BlockPoiseDamageModifier para resistir daño al temple o equilibrio.
-El sistema detecta desde qué dirección proviene el ataque usando AngleUtilities y decide si 
debe bloquearse o no. Además, lanza partículas visuales y emite el evento OnBlock para que 
otros sistemas (como sonido o cámara) puedan reaccionar.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class Block : WeaponComponent<BlockData, AttackBlock>
    {
        // Evento que se lanza cuando un ataque es bloqueado exitosamente. El parámetro es el GameObject del atacante.
        public event Action<GameObject> OnBlock;

        // Referencias a componentes del núcleo del jugador
        private DamageReceiver damageReceiver;
        private KnockBackReceiver knockBackReceiver;
        private PoiseDamageReceiver poiseDamageReceiver;

        // Modificadores que se aplican durante la ventana de bloqueo
        private DamageModifier damageModifier;
        private BlockKnockBackModifier knockBackModifier;
        private BlockPoiseDamageModifier poiseDamageModifier;

        // Referencias auxiliares
        private CoreSystem.Movement movement;
        private ParticleManager particleManager;

        // Flags de control de ventana de bloqueo
        private bool isBlockWindowActive;
        private bool shouldUpdate;

        // Tiempo exacto en que se debe iniciar/detener la ventana
        private float nextWindowTriggerTime;

        // Activa la ventana de bloqueo y aplica los modificadores correspondientes.
        private void StartBlockWindow()
        {
            isBlockWindowActive = true;
            shouldUpdate = false;

            damageModifier.OnModified += HandleModified;

            damageReceiver.Modifiers.AddModifier(damageModifier);
            knockBackReceiver.Modifiers.AddModifier(knockBackModifier);
            poiseDamageReceiver.Modifiers.AddModifier(poiseDamageModifier);
        }

        // Desactiva la ventana de bloqueo y remueve los modificadores.
        private void StopBlockWindow()
        {
            isBlockWindowActive = false;
            shouldUpdate = false;

            damageModifier.OnModified -= HandleModified;

            damageReceiver.Modifiers.RemoveModifier(damageModifier);
            knockBackReceiver.Modifiers.RemoveModifier(knockBackModifier);
            poiseDamageReceiver.Modifiers.RemoveModifier(poiseDamageModifier);
        }

        // Determina si un ataque fue bloqueado, según el ángulo desde donde vino el atacante.
        private bool IsAttackBlocked(Transform source, out DirectionalInformation directionalInformation)
        {
            // Calcula el ángulo entre el jugador y el atacante, tomando en cuenta la dirección del jugador
            var angleOfAttacker = AngleUtilities.AngleFromFacingDirection(Core.Root.transform, source, movement.FacingDirection);

            return currentAttackData.IsBlocked(angleOfAttacker, out directionalInformation);
        }

        // Método llamado cuando el modificador de daño detecta un bloqueo.
        // Lanza partículas y emite el evento OnBlock.
        private void HandleModified(GameObject source)
        {
            particleManager.StartWithRandomRotation(currentAttackData.Particles, currentAttackData.ParticlesOffset);
            OnBlock?.Invoke(source);
        }

        // Maneja los eventos de entrada en fases de ataque para controlar cuándo activar la ventana de bloqueo.
        private void HandleEnterAttackPhase(AttackPhases phase)
        {
            shouldUpdate = isBlockWindowActive ? currentAttackData.BlockWindowEnd.TryGetTriggerTime(phase, out nextWindowTriggerTime) : currentAttackData.BlockWindowStart.TryGetTriggerTime(phase, out nextWindowTriggerTime);
        }

        protected override void Start()
        {
            base.Start();

            // Obtener referencias a componentes del núcleo
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            particleManager = Core.GetCoreComponent<ParticleManager>();

            knockBackReceiver = Core.GetCoreComponent<KnockBackReceiver>();
            damageReceiver = Core.GetCoreComponent<DamageReceiver>();
            poiseDamageReceiver = Core.GetCoreComponent<PoiseDamageReceiver>();

            // Crear los modificadores que usarán los receptores
            damageModifier = new DamageModifier(IsAttackBlocked);
            knockBackModifier = new BlockKnockBackModifier(IsAttackBlocked);
            poiseDamageModifier = new BlockPoiseDamageModifier(IsAttackBlocked);

            // Suscribirse al evento que marca el cambio de fase de ataque
            AnimationEventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        // Controla el momento exacto de iniciar o detener la ventana de bloqueo.
        private void Update()
        {
            if (!shouldUpdate || !IsPastTriggerTime())
                return;

            if (isBlockWindowActive)
                StopBlockWindow();
            else
                StartBlockWindow();
        }

        private bool IsPastTriggerTime()
        {
            return Time.time >= nextWindowTriggerTime;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
        }
    }
}
