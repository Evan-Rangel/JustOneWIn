using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackTargeter : AttackData
    {
        [field: SerializeField] public Rect Area { get; private set; }
        [field: SerializeField] public LayerMask DamageableLayer { get; private set; }
    }
}
