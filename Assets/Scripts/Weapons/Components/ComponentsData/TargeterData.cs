using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class TargeterData : ComponentData<AttackTargeter>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Targeter);
        }
    }
}
