using System.Collections;
using System.Collections.Generic;
using Avocado.Weapons.Components;
using UnityEngine;

namespace Avocado.Weapons.Modifiers
{
    public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}
