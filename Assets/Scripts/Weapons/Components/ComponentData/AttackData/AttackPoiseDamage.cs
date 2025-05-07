using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackPoiseDamage representa un tipo específico de daño que afecta la "poise" de un 
personaje, es decir, su capacidad para resistir interrupciones o aturdimientos al recibir golpes. 
Por ejemplo, un personaje con alta poise puede seguir atacando aunque lo golpeen, pero si el valor 
de Amount supera cierto umbral, puede ser interrumpido. Este valor probablemente se acumula y 
se compara contra un sistema de resistencia o equilibrio en el personaje receptor del ataque.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackPoiseDamage : AttackData
    {
        // Cantidad de daño a la "poise" (estabilidad o resistencia al aturdimiento) que este ataque inflige.
        [field: SerializeField] public float Amount { get; private set; }
    }
}
