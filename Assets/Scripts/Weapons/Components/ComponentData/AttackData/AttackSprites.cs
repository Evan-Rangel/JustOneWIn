using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este sistema permite asociar diferentes sprites a distintas fases de un ataque (como preparación, 
ejecución, recuperación). Esto es útil para representar visualmente la transición entre fases con 
sprites únicos, permitiendo mayor claridad y feedback visual al jugador. Por ejemplo:
-Fase de carga: sprite con el personaje cargando energía.
-Fase de ataque: sprite con la animación del ataque activo.
-Fase de recuperación: sprite con el personaje en cooldown.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackSprites : AttackData
    {
        // Arreglo de sprites asociados a fases específicas del ataque.
        [field: SerializeField] public PhaseSprites[] PhaseSprites { get; private set; }
    }

    [Serializable]
    public struct PhaseSprites
    {
        // Fase del ataque a la que corresponden los sprites (por ejemplo: preparación, ejecución, recuperación).
        [field: SerializeField] public AttackPhases Phase { get; private set; }

        // Arreglo de sprites que se usarán durante esa fase del ataque.
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}
