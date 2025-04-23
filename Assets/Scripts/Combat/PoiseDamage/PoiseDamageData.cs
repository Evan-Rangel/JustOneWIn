using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Combat.PoiseDamage
{
    public class PoiseDamageData
    {
        public float Amount { get; private set; }
        public GameObject Source { get; private set; }

        public PoiseDamageData(float amount, GameObject source)
        {
            Amount = amount;
            Source = source;
        }

        public void SetAmount(float amount) => Amount = amount;
    }
}
