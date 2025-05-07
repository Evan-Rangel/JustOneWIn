using System.Collections;
using System.Collections.Generic;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este Script es el estado PlayerLedgeClimbState representa el comportamiento del jugador cuando 
se cuelga de una cornisa y puede trepar, soltarse o hacer un salto desde la pared.
Funciones clave:
Enter(): Posiciona al jugador en el punto correcto de inicio para colgarse.
LogicUpdate(): Si el jugador se mueve hacia la cornisa, se reproduce la animación de trepar.
Si presiona hacia abajo, se suelta y cae (InAirState).
Si presiona salto, ejecuta un salto de pared.
CheckForSpace(): Verifica si hay techo para evitar que el jugador trepe a una zona donde no cabe.
---------------------------------------------------------------------------------------------*/

public class PlayerLedgeClimbState : PlayerState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private Vector2 detectedPos; // Posición detectada al colgarse
    private Vector2 cornerPos;   // Posición de la esquina de la cornisa
    private Vector2 startPos;    // Posición donde empieza la animación de trepar
    private Vector2 stopPos;     // Posición donde termina la animación de trepar
    private Vector2 workspace;   // Variable auxiliar para cálculos de posición

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    // Llamado cuando finaliza una animación
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("climbLedge", false);
    }

    // Llamado en un evento de animación intermedio (para activar que está colgado)
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging = true;
    }

    // Al entrar al estado
    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();              // Detiene el movimiento
        player.transform.position = detectedPos;  // Coloca al jugador donde se detectó el borde
        cornerPos = DetermineCornerPosition();    // Calcula la esquina del borde

        // Calcula posición inicial y final de la animación de trepar
        startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x),
                     cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.stopOffset.x),
                    cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;     // Coloca al jugador al inicio de la animación
    }

    // Al salir del estado
    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        // Si trepó exitosamente, lo coloca en la posición final
        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    // Lógica que se ejecuta en cada frame
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Si la animación terminó, pasar a otro estado
        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            // Recolecta inputs del jugador
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            Movement?.SetVelocityZero();
            player.transform.position = startPos;

            // Inicia el trepado si se mueve hacia la cornisa
            if (xInput == Movement.FacingDirection && isHanging && !isClimbing)
            {
                CheckForSpace(); // Verifica que no haya techo bloqueando
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true); // Activa animación
            }
            // Si presiona hacia abajo, se suelta
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            // Si presiona salto, hace un wall jump
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    // Define la posición desde la que se cuelga
    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    // Verifica si hay espacio arriba antes de trepar
    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(
            cornerPos + (Vector2.up * 0.015f) + (Vector2.right * Movement.FacingDirection * 0.015f),
            Vector2.up,
            playerData.standColliderHeight,
            CollisionSenses.WhatIsGround
        );

        player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }

    // Determina la esquina exacta de la cornisa usando raycasts
    private Vector2 DetermineCornerPosition()
    {
        // Raycast horizontal para encontrar el borde del muro
        RaycastHit2D xHit = Physics2D.Raycast(
            CollisionSenses.WallCheck.position,
            Vector2.right * Movement.FacingDirection,
            CollisionSenses.WallCheckDistance,
            CollisionSenses.WhatIsGround
        );

        float xDist = xHit.distance;
        workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);

        // Raycast vertical desde el borde para encontrar la superficie del suelo
        RaycastHit2D yHit = Physics2D.Raycast(
            CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(workspace),
            Vector2.down,
            CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f,
            CollisionSenses.WhatIsGround
        );

        float yDist = yHit.distance;

        // Devuelve la posición exacta de la esquina
        workspace.Set(
            CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection),
            CollisionSenses.LedgeCheckHorizontal.position.y - yDist
        );

        return workspace;
    }
}
