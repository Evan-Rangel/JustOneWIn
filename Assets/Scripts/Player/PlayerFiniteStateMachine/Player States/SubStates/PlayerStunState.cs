using Avocado.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.FSM
{
    public class PlayerStunState : PlayerState
    {
        private readonly Movement movement;

        public PlayerStunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            movement = core.GetCoreComponent<Movement>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            movement.SetVelocityX(0f);

            if (Time.time >= startTime + playerData.stunTime)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
