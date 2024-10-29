using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    [Header("Weapon Stats")]
    public string attackName;

    public float movementSpeed;
    public float damageAmount;
    public float KnockbackStrength;

    public Vector2 knockbackAngle;
}

