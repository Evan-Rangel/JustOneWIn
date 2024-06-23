using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    //Charge Stats
    public float chargeSpeed = 6.0f;
    public float chargeTime = 2.0f;
}
