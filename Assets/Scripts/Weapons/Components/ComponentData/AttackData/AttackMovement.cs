using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackMovement contiene los datos que definen el movimiento asociado a un ataque. 
Específicamente, se usa para mover al atacante o proyectil en una dirección a 
una velocidad constante. Esto es útil, por ejemplo, en ataques que implican un dash o 
proyectiles que deben avanzar en cierta dirección.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackMovement : AttackData
    {
        // Dirección en la que se moverá el personaje o proyectil al ejecutar el ataque.
        // Por ejemplo, (1, 0) sería hacia la derecha, (0, 1) hacia arriba.
        [field: SerializeField] public Vector2 Direction { get; private set; }

        // Velocidad a la que se moverá en la dirección indicada.
        [field: SerializeField] public float Velocity { get; private set; }
    }
}
