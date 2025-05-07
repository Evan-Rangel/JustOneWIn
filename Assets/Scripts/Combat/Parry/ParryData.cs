using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script contiene información que se transfiere durante una acción de parry.
---------------------------------------------------------------------------------------------*/

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
