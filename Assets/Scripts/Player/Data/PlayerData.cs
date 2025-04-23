using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    //---Idle State Vars--//

    //---Move State Vars--//
    [Header("Move State")]
    public float movementVelocity = 10f;

    //---Crouch State Vars--//
    [Header("Crouch State")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.8f;
    public float standColliderHeight = 1.6f;

    //---Jump State Vars--//
    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    //---Wall Jump State Vars--//
    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    //---Air State Vars--//
    [Header("Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    //---Wall Slide State Vars--//
    [Header("Wall Slide State")]
    public float wallSlideVelocity = 2f;

    //---Wall Climb State Vars--//
    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;

    //---Ledge Climb State Vars--//
    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    //---Dash State Vars--//
    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distBetweenAfterImages = 0.5f;

    [Header("Stun State")]
    public float stunTime = 2f;
}
