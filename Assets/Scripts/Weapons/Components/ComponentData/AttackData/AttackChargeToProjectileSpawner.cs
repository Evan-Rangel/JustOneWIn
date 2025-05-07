using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackChargeToProjectileSpawner define una propiedad usada en ataques que convierten 
la energía de una carga en proyectiles. La única propiedad AngleVariation permite ajustar cuánto 
se desvían los proyectiles de su ángulo base, haciendo que puedan salir dispersos en un rango 
específico. Esto es útil para crear efectos como disparos en abanico o menos precisos a medida 
que se carga más.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackChargeToProjectileSpawner : AttackData
    {
        // Variación del ángulo en grados para los proyectiles que se disparan tras la carga, permite más dispersión en el disparo.
        [field: SerializeField, Range(0f, 360f)] public float AngleVariation { get; private set; }
    }
}
