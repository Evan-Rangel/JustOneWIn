using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackDamage define la cantidad de daño (Amount) que se aplica durante un ataque. 
Hereda de AttackData, lo cual la hace compatible con el sistema general de componentes de ataque 
del framework. El valor es privado para la modificación directa, pero puede configurarse desde 
el editor y ser accedido mediante una propiedad pública de solo lectura.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackDamage : AttackData
    {
        // Cantidad de daño que inflige el ataque.
        [field: SerializeField] public float Amount { get; private set; }
    }
}
