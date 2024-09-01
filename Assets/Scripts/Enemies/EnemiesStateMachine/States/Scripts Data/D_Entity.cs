using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    //Enemy Stats
    public float maxHealth = 30f;
    public float damageHopSpeed = 3.0f;

    //Enemy Detectors
    public float wallCheckDistance = 0.7f;
    public float ledgeCheckDistance = 0.4f;
    public float closeRangeActionDistance = 1.0f;
    public float groundCheckRadius = 0.3f;

    //Enemy Detect Player
    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    //Enemy Stun
    public float stunResistance = 3.0f;
    public float stunRecoveryTime = 2.0f;

    //Enemy Particles
    public GameObject hitParticle;
    
    //Enemy Layers
    [Tooltip("Put the layer called -Ground-, that layer only detects places to walk")]
    public LayerMask whatIsGround;
    [Tooltip("Put the layer called -Player-")]
    public LayerMask whatIsPlayer;
}
