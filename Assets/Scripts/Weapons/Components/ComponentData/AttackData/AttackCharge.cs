using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackCharge contiene los datos que controlan cómo funciona un ataque con sistema de 
carga (charge attack). Define el tiempo necesario para cargar, cuántas veces se puede cargar un 
ataque y qué efectos visuales se deben mostrar durante el proceso. Por ejemplo, cuando se añade 
una carga se muestra una partícula, y otra diferente cuando se alcanza la carga máxima. Además, 
especifica una posición relativa al jugador para mostrar las partículas correctamente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackCharge : AttackData
    {
        // Tiempo necesario para completar una sola carga.
        [field: SerializeField] public float ChargeTime { get; private set; }

        // Cantidad de cargas con las que inicia el ataque (valor entre 0 y 1).
        [field: SerializeField, Range(0, 1)] public int InitialChargeAmount { get; private set; }

        // Número máximo de veces que se puede cargar el ataque.
        [field: SerializeField] public int NumberOfCharges { get; private set; }

        // Prefab de partículas que se instancia cada vez que se añade una carga.
        [field: SerializeField] public GameObject ChargeIncreaseIndicatorParticlePrefab { get; private set; }

        // Prefab de partículas que se instancia cuando se alcanza la carga máxima.
        [field: SerializeField] public GameObject FullyChargedIndicatorParticlePrefab { get; private set; }

        // Desplazamiento relativo al transform del jugador para instanciar las partículas.
        [field: SerializeField] public Vector2 ParticlesOffset { get; private set; }
    }
}
