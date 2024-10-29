using Avocado.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Avocado.CoreSystem
{
    public class PlayerAttackState : PlayerAbilityState
    {
        //---PlayerAttackState Vars---//
        #region PlayerAttackState Vars
        private Weapon weapon;
        #endregion

        #region Values
        private int inputIndex;
        #endregion

        #region Flags
        #endregion

        //---PlayerAttackState Construct---//
        #region Construct
        public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, Weapon weapon, CombatInputs input) : base(player, stateMachine, playerData, animBoolName)
        {
            this.weapon = weapon;

            inputIndex = (int)input;

            weapon.OnExit += ExitHandler;
        }
        #endregion

        #region Override Functions
        public override void Enter()
        {
            base.Enter();

            weapon.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            weapon.CurrentInput = player.InputHandler.AttackInputs[inputIndex];
        }

        private void ExitHandler()
        {
            AnimationFinishTrigger();
            isAbilityDone = true;
        }
        #endregion
    }
}