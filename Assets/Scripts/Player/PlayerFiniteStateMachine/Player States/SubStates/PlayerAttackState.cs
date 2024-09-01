using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAttackState : PlayerAbilityState
{
    //---PlayerAttackState Vars---//
    #region PlayerAttackState Vars
    private Weapon weapon;
    #endregion

    #region Values
    private float velocityToSet;

    private int xInput;
    #endregion

    #region Flags
    private bool setVelocity;
    private bool shouldCheckFlip;
    #endregion

    //---PlayerAttackState Construct---//
    #region Construct
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    #endregion

    //---Override Functions---//
    #region Override Functions
    public override void Enter()
    {
        base.Enter();

        //Set velocity to false to avoid giving speed before activate the weapon
        setVelocity = false;

        //Call the Enter function on "Weapon"
        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        //Call the Exit function on "Weapon"
        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Equal with our real input
        xInput = player.InputHandler.NormInputX;

        player.CheckIfShouldFlip(xInput);//This allow to flip when attack

        //Condition that check if we can flip base on the xInput
        if (shouldCheckFlip)
        {
            player.CheckIfShouldFlip(xInput);
        }

        //Condition that give the velocity with reference the facingdirection
        if(setVelocity)
        {
            player.SetVelocityX(velocityToSet * player.FacingDirection);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
    #endregion

    //---Other Functions---//
    #region Other Functions
    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);//With this the weapons has a way to tell the state when the animation is finished
    }

    public void SetPlayerVelocity(float velocity)
    {
        player.SetVelocityX(velocity * player.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
    #endregion

    #region Animation Triggers
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }
    #endregion
}
