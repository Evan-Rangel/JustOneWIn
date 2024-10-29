using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class KnockBackData : ComponentData<AttackKnockBack>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(KnockBack);
        }
    }
}
