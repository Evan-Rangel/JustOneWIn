using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
AnimationEventHandler actúa como intermediario entre las animaciones y la lógica del juego. 
Permite que eventos definidos en la animación (mediante los "Animation Events") disparen 
funciones que otros scripts pueden escuchar, como iniciar movimiento, terminar una animación, 
activar efectos visuales o iniciar fases de ataque. Esto facilita una programación modular y 
flexible para sistemas de combate complejos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    // Esta clase permite conectar eventos de animación con lógica del juego (como movimiento, ataque, etc.)
    public class AnimationEventHandler : MonoBehaviour
    {
        // Eventos que otros scripts pueden escuchar y usar cuando ocurren ciertos eventos animados (en la animaciones como tal)
        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;
        public event Action OnMinHoldPassed;

        // Este evento se dispara en un punto específico de la animación para indicar que el jugador debe soltar y volver a presionar el input.
        //Se usa normalmente cuando ocurre la acción (como disparar un arco o golpear con una espada).
        public event Action OnUseInput;

        public event Action OnEnableInterrupt; // Permite que una animación sea interrumpida

        public event Action<bool> OnSetOptionalSpriteActive; // Activa o desactiva un sprite opcional (como un efecto visual)
        public event Action<bool> OnFlipSetActive; // Activa o desactiva el flip (volteo) del sprite

        public event Action<AttackPhases> OnEnterAttackPhase; // Dispara cuando se entra en una nueva fase de ataque

        // Eventos usados para definir ventanas específicas dentro de la animación (como una ventana de bloqueo o de parry)
        // Estas se identifican mediante un enum llamado AnimationWindows.
        public event Action<AnimationWindows> OnStartAnimationWindow;
        public event Action<AnimationWindows> OnStopAnimationWindow;

        // Métodos llamados desde eventos en la animación (Animation Events)

        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
        private void UseInputTrigger() => OnUseInput?.Invoke();

        private void SetOptionalSpriteEnabled() => OnSetOptionalSpriteActive?.Invoke(true);
        private void SetOptionalSpriteDisabled() => OnSetOptionalSpriteActive?.Invoke(false);

        private void SetFlipActive() => OnFlipSetActive?.Invoke(true);
        private void SetFlipInactive() => OnFlipSetActive?.Invoke(false);

        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);

        private void StartAnimationWindow(AnimationWindows window) => OnStartAnimationWindow?.Invoke(window);
        private void StopAnimationWindow(AnimationWindows window) => OnStopAnimationWindow?.Invoke(window);

        private void EnableInterrupt() => OnEnableInterrupt?.Invoke();
    }
}
