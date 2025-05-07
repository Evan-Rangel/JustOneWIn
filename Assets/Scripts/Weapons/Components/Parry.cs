using System;
using Avocado.CoreSystem;
using Avocado.Utilities;
using Avocado.Weapons.Modifiers;
using UnityEngine;
using static Avocado.Combat.Parry.CombatParryUtilities;

/*---------------------------------------------------------------------------------------------
Este componente permite al jugador realizar un parry, es decir, repeler un ataque enemigo si 
ocurre dentro de una ventana específica y desde un ángulo válido. Usa modificadores (DamageModifier
, etc.) para detectar si el ataque puede bloquearse. Abre y cierra dinámicamente una “ventana de parry” 
basada en las fases del ataque. Si un ataque es parried exitosamente, se dispara una animación, 
se notifica al enemigo, y se muestran partículas. El diseño modular permite combinarlo fácilmente 
con otros sistemas como animaciones, partículas y física.
Este componente funciona de forma similar al de "Block".
Activa una ventana temporal durante la cual, si se recibe daño, se puede hacer parry al atacante.
Usa modificadores para detectar el ataque y aplica retroceso, daño a la postura, y efectos visuales.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class Parry : WeaponComponent<ParryData, AttackParry>
    {
        public event Action<GameObject> OnParry;

        // Referencias a componentes del sistema principal (core)
        private DamageReceiver damageReceiver;
        private KnockBackReceiver knockBackReceiver;
        private PoiseDamageReceiver poiseDamageReceiver;

        // Modificadores que controlan la lógica del parry
        private DamageModifier damageModifier;
        private BlockKnockBackModifier knockBackModifier;
        private BlockPoiseDamageModifier poiseDamageModifier;

        private CoreSystem.Movement movement;
        private ParticleManager particleManager;

        // Estados de control de ventana de parry
        private bool isBlockWindowActive;
        private bool shouldUpdate;
        private float nextWindowTriggerTime;

        // Inicia la ventana de parry, aplicando los modificadores a los sistemas de defensa.
        private void StartParryWindow()
        {
            isBlockWindowActive = true;
            shouldUpdate = false;

            damageModifier.OnModified += HandleParry;

            damageReceiver.Modifiers.AddModifier(damageModifier);
            knockBackReceiver.Modifiers.AddModifier(knockBackModifier);
            poiseDamageReceiver.Modifiers.AddModifier(poiseDamageModifier);
        }

        // Finaliza la ventana de parry, removiendo los modificadores.
        private void StopParryWindow()
        {
            isBlockWindowActive = false;
            shouldUpdate = false;

            damageModifier.OnModified += HandleParry;

            damageReceiver.Modifiers.RemoveModifier(damageModifier);
            knockBackReceiver.Modifiers.RemoveModifier(knockBackModifier);
            poiseDamageReceiver.Modifiers.RemoveModifier(poiseDamageModifier);
        }

        // Asegura que al salir del ataque se limpien los modificadores.
        protected override void HandleExit()
        {
            base.HandleExit();

            damageReceiver.Modifiers.RemoveModifier(damageModifier);
            knockBackReceiver.Modifiers.RemoveModifier(knockBackModifier);
            poiseDamageReceiver.Modifiers.RemoveModifier(poiseDamageModifier);
        }

        // Determina si un ataque recibido fue desde un ángulo válido para hacer parry.
        private bool IsAttackParried(Transform source, out DirectionalInformation directionalInformation)
        {
            var angleOfAttacker = AngleUtilities.AngleFromFacingDirection(
                Core.Root.transform,
                source,
                movement.FacingDirection
            );

            return currentAttackData.IsBlocked(angleOfAttacker, out directionalInformation);
        }

        // Se llama cuando el modificador detecta un ataque dentro de la ventana de parry. Informa al atacante que fue "parried", activa la animación y partículas.
        private void HandleParry(GameObject parriedGameObject)
        {
            if (!TryParry(parriedGameObject, new Combat.Parry.ParryData(Core.Root), out _, out _))
                return;

            weapon.Anim.SetTrigger("parry");

            OnParry?.Invoke(parriedGameObject);

            particleManager.StartWithRandomRotation(currentAttackData.Particles, currentAttackData.ParticlesOffset);
        }

        // Controla cuándo iniciar o detener la ventana de parry en función de la fase de ataque.
        private void HandleEnterAttackPhase(AttackPhases phase)
        {
            shouldUpdate = isBlockWindowActive
                ? currentAttackData.ParryWindowEnd.TryGetTriggerTime(phase, out nextWindowTriggerTime)
                : currentAttackData.ParryWindowStart.TryGetTriggerTime(phase, out nextWindowTriggerTime);
        }

        // Obtiene referencias a los componentes Core y crea los modificadores.
        protected override void Start()
        {
            base.Start();

            damageReceiver = Core.GetCoreComponent<DamageReceiver>();
            knockBackReceiver = Core.GetCoreComponent<KnockBackReceiver>();
            poiseDamageReceiver = Core.GetCoreComponent<PoiseDamageReceiver>();

            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            particleManager = Core.GetCoreComponent<ParticleManager>();

            damageModifier = new DamageModifier(IsAttackParried);
            knockBackModifier = new BlockKnockBackModifier(IsAttackParried);
            poiseDamageModifier = new BlockPoiseDamageModifier(IsAttackParried);

            AnimationEventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        // Verifica constantemente si ya se alcanzó el momento de abrir o cerrar la ventana de parry.
        private void Update()
        {
            if (!shouldUpdate || !IsPastTriggerTime())
                return;

            if (isBlockWindowActive)
                StopParryWindow();
            else
                StartParryWindow();
        }

        // Determina si se alcanzó el tiempo programado para cambiar de estado.
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
