using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackChargeToProjectileSpawner : AttackData
    {
        [field: SerializeField, Range(0f, 360f)] public float AngleVariation { get; private set; }
    }
}
