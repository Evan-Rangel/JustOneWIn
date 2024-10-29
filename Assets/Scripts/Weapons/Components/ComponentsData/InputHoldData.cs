using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class InputHoldData : ComponentData
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(InputHold);
        }
    }
}
