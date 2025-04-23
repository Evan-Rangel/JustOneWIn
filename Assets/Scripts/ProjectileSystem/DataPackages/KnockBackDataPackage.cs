using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class KnockBackDataPackage : ProjectileDataPackage
    {
        [field: SerializeField] public float Strength;
        [field: SerializeField] public Vector2 Angle;
    }
}
