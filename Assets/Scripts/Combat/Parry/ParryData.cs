using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Combat.Parry
{
    public class ParryData
    {
        public GameObject Source { get; private set; }

        public ParryData(GameObject source)
        {
            Source = source;
        }
    }
}
