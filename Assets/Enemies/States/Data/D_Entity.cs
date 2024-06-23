using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    //Enemy Values
    public float wallCheckDistance = 0.7f;
    public float ledgeCheckDistance = 0.4f;
    //Enemy Detect Player
    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;
    
    //Enemy Layers
    [Tooltip("Put the layer called -Ground-, that layer only detects places to walk")]
    public LayerMask whatIsGround;
    [Tooltip("Put the layer called -Player-")]
    public LayerMask whatIsPlayer;
}
