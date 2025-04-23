using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class DrawModifierDataPackage : ProjectileDataPackage
    {
        public float DrawPercentage
        {
            get => drawPercentage;
            set => drawPercentage = Mathf.Clamp01(value);
        }

        private float drawPercentage;
    }
}
