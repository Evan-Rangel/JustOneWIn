using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using TMPro;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define una clase base abstracta para representar los distintos estados del jugador 
(como moverse, saltar, atacar, etc.) dentro de una Máquina de Estados Finita (FSM). Su propósito 
es estandarizar cómo se comportan todos los estados, proporcionando métodos comunes como Enter,
Exit, LogicUpdate, PhysicsUpdate, y DoChecks.
Cada subestado como PlayerJumpState o PlayerIdleState hereda de esta clase y sobreescribe los
métodos necesarios para definir su comportamiento único. Además, gestiona animaciones de 
manera automática, activando y desactivando parámetros booleanos en el Animator, y permite 
reaccionar a eventos de animación mediante AnimationTrigger y AnimationFinishTrigger.
---------------------------------------------------------------------------------------------*/

public class PlayerState
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    // Flags que indican si la animación ha terminado o si se está saliendo del estado
    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    // Nombre del parámetro booleano de animación (ej: "idle", "jump") que se activa en este estado
    private string animBoolName;
    protected PlayerObjectController playerController;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;

        // Se obtiene el componente Core desde el jugador
        core = player.Core;
        playerController= player.GetComponent<PlayerObjectController>();
    }

    // Se llama al entrar al estado
    public virtual void Enter()
    {
        DoChecks(); // Se realizan verificaciones iniciales (sobrescribibles por estados hijos)
        player.Anim.SetBool(animBoolName, true); // Se activa la animación correspondiente
        //playerController.NetworkSetBool(animBoolName, true); // Se sincroniza la animación en red
        startTime = Time.time; // Se guarda el tiempo de entrada al estado
        isAnimationFinished = false; // Se resetea el flag de animación
        isExitingState = false; // Se indica que aún no se está saliendo
    }

    // Se llama al salir del estado
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false); // Se desactiva la animación
       // playerController.NetworkSetBool(animBoolName, false); // Se sincroniza la animación en red

        isExitingState = true; // Se indica que se está saliendo del estado
    }

    // Se llama en cada frame (Update), útil para lógica como leer inputs
    public virtual void LogicUpdate()
    {

    }

    // Se llama en cada frame de física (FixedUpdate), útil para mover al jugador o detectar colisiones
    public virtual void PhysicsUpdate()
    {
        DoChecks(); // Verificaciones físicas o de entorno
    }

    // Método que pueden sobrescribir los estados hijos para hacer sus propias validaciones (como si está tocando el suelo)
    public virtual void DoChecks() { }

    // Se llama desde un evento de animación para indicar que se alcanzó cierto punto dentro de una animación (como el momento de golpear)
    public virtual void AnimationTrigger() { }

    // Se llama desde un evento de animación para indicar que la animación ha terminado
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
