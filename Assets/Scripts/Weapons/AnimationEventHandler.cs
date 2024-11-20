using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        #region Events
        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;
        public event Action OnMinHoldPassed;

        public event Action<AttackPhases> OnEnterAttackPhase;
        #endregion

        #region Animation Functions
        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
        #endregion
    }   
}
