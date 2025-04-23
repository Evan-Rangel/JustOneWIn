using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Weapons.Components
{
    public class OptionalSpriteData : ComponentData<AttackOptionalSprite>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(OptionalSprite);
        }
    }
}
