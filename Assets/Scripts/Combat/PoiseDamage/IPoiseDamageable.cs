using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Combat.PoiseDamage
{
    public interface IPoiseDamageable
    {
        void DamagePoise(PoiseDamageData data);
    }
}
