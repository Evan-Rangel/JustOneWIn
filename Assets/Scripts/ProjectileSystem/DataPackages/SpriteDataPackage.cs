using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class SpriteDataPackage : ProjectileDataPackage
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
