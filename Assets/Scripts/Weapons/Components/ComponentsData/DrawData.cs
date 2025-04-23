using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class DrawData : ComponentData<AttackDraw>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Draw);
        }
    }
}
