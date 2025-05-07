using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackTargeter se encarga de definir un área de búsqueda (usando un Rect) para detectar 
objetivos durante un ataque, y filtra esos objetivos usando una LayerMask para asegurarse de que 
solo se consideren entidades que puedan recibir daño. Por ejemplo:
-El Rect puede usarse para representar un área de efecto de un ataque cuerpo a cuerpo o un hechizo.
-La LayerMask evita que el ataque detecte objetos irrelevantes como el suelo o decoraciones.
Esto es útil para:
-Ataques con área de efecto (AOE).
-Hechizos de selección automática de objetivos.
-Golpes en un cono, zona frontal, etc.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackTargeter : AttackData
    {
        // Define el área rectangular (en coordenadas locales o globales) donde se buscarán los objetivos.
        [field: SerializeField] public Rect Area { get; private set; }

        // Define qué capas pueden ser detectadas como objetivos válidos (usualmente enemigos u objetos dañables).
        [field: SerializeField] public LayerMask DamageableLayer { get; private set; }
    }
}
