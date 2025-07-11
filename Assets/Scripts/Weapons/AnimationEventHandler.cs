using Mirror;
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
    public class AnimationEventHandler : NetworkBehaviour
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

        public event Action OnRequestAttackAction;
        private void AttackActionTrigger()=> OnRequestAttackAction?.Invoke();
        [ClientRpc]
        public void AttackAction() => OnAttackAction?.Invoke(); 


        // Métodos llamados desde eventos en la animación (Animation Events)
        public event Action OnRequesFinishtAttackAction;
        private void FinishAttackActionTrigger() => OnRequesFinishtAttackAction?.Invoke();
        [ClientRpc]
        public void AnimationFinishedTrigger() => OnFinish?.Invoke();


        public event Action OnRequestStartMovement;
        //private void StartMovementTrigger() => OnRequestStartMovement?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        [ClientRpc]
        public void StartMovement() => OnStartMovement?.Invoke();


        public event Action OnRequestStopMovement;
       // private void StopMovementTrigger() => OnRequestStopMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        [ClientRpc]
        public void StopMovement() => OnStopMovement?.Invoke();


        public event Action OnRequestMinHoldPassed;
        private void MinHoldPassedTrigger() => OnRequestMinHoldPassed?.Invoke();
        [ClientRpc]
        public void HoldPassedTrigger()=>OnMinHoldPassed?.Invoke(); 


        public event Action OnRequestUseInput;
        private void UseInputTrigger() => OnRequestUseInput?.Invoke();
        [ClientRpc]
        public void UseInput() => OnUseInput?.Invoke();


        public event Action OnRequestSetOptionalSpriteEnabled;
        private void SetOptionalSpriteEnabled() => OnRequestSetOptionalSpriteEnabled?.Invoke();
        [ClientRpc]
        public void OptionalSpriteEnabled() => OnSetOptionalSpriteActive?.Invoke(true);

            
        public event Action OnRequestSetOptionalSpriteDisabled;
        private void SetOptionalSpriteDisabled() => OnRequestSetOptionalSpriteDisabled?.Invoke();
        [ClientRpc]
        public void OptionalSpriteDisabled() => OnSetOptionalSpriteActive?.Invoke(false);


        public event Action OnRequestFlipSetActive;
        private void SetFlipActive() => OnRequestFlipSetActive?.Invoke();
        [ClientRpc]
        public void FlipSetActive() => OnFlipSetActive?.Invoke(true);


        public event Action OnRequestFlipSetInactive;
        private void SetFlipInactive() => OnRequestFlipSetInactive?.Invoke();
        [ClientRpc]
        public void FlipSetInactive() => OnFlipSetActive?.Invoke(false);


        public event Action<int> OnRequestEnterAttackPhase;
        //private void EnterAttackPhase(AttackPhases phase) => OnRequestEnterAttackPhase?.Invoke((int) phase);
          //private void EnterAttackPhase(AttackPhases phase) => OnRequestEnterAttackPhase?.Invoke((int) phase);
          private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
        [ClientRpc]
        public void EnterAttack(int phase) => OnEnterAttackPhase?.Invoke((AttackPhases)phase);


        public event Action<int> OnRequestStartAnimationWindow;
        private void StartAnimationWindow(AnimationWindows window) => OnRequestStartAnimationWindow?.Invoke((int)window);
        [ClientRpc]
        public void StartAnimationWindow(int window) => OnStartAnimationWindow?.Invoke((AnimationWindows)window);


        public event Action<int> OnRequestStopAnimationWindow;
        private void StopAnimationWindow(AnimationWindows window) => OnRequestStopAnimationWindow?.Invoke((int)window);
        [ClientRpc]
        public void StopAnimationWindow(int window) => OnStopAnimationWindow?.Invoke((AnimationWindows)window);


        public event Action OnRequestEnableInterrupt;
        private void EnableInterrupt() => OnRequestEnableInterrupt?.Invoke();
        [ClientRpc]
        public void EnableInterruptTrigger() => OnEnableInterrupt?.Invoke(); 
    }
}
