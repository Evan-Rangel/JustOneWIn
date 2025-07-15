using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;
using Mirror;
public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected Core core;    

    public float startTime { get; protected set; }

    protected string animBoolName;

    public State(Entity etity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = etity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = entity.Core;
    }

    public virtual void Enter()
    {
        if (!entity.isServer) return;
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit()
    {
        if (!entity.isServer) return;
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        if (!entity.isServer) return;
    }

    public virtual void PhysicsUpdate()
    {
        if (!entity.isServer) return;
        DoChecks();
    }

    public virtual void DoChecks()
    {
        if (!entity.isServer) return;
    }
}
