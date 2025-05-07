using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackKnockBack representa los datos de retroceso que se aplican a un enemigo cuando 
es golpeado por un ataque. Define la dirección del empuje (Angle) y la magnitud de ese empuje 
(Strength). Es útil para efectos de impacto físicos, como empujar a los enemigos hacia atrás 
o hacer que salgan volando.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackKnockBack : AttackData
    {
        // Dirección del knockback en coordenadas 2D (por ejemplo: (1,1) empuja diagonalmente hacia arriba a la derecha).
        [field: SerializeField] public Vector2 Angle { get; private set; }

        // Fuerza o intensidad del retroceso. Un valor mayor empuja más fuerte al objetivo.
        [field: SerializeField] public float Strength { get; private set; }
    }
}
