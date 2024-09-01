using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //---Player States Vars---//
    #region References Vars
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    #endregion

    #region Animation Vars
    //-Animation Names-//
    private string animBoolName;

    //-Animation Flags-//
    protected bool isAnimationFinished;
    protected bool isExitingState;
    #endregion

    #region Other Vars
    //-Floats-//
    protected float startTime;
    #endregion
    //-----------------//

    //---PlayerState Construct---//
    #region PlayerState Construct
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        //Equal refers
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }
    #endregion

    //---Virtual Override Functions---//
    #region Virtual Override Functions
    public virtual void Enter()
    {
        DoChecks();
        //Set Animator
        player.Animator.SetBool(animBoolName, true);
        //Set StartTime
        startTime = Time.time;
        Debug.Log(animBoolName);
        //Set Flags
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        //Set Animator
        player.Animator.SetBool(animBoolName, false);
        //Set Flags
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
    #endregion

    //---Virtual Animation Functions---//
    #region VirtualAnimation Functions
    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    #endregion
}
