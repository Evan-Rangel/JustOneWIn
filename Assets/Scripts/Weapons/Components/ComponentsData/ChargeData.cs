using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class ChargeData : ComponentData<AttackCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Charge);
        }
    }
}
