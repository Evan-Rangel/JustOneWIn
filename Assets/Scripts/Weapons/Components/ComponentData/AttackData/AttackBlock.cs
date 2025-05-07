using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
AttackBlock define los datos necesarios para que un ataque tenga la capacidad de bloquear. El 
bloqueo se basa en regiones direccionales (DirectionalInformation[]), que definen qué ángulos se 
consideran bloqueables. Además, se puede configurar una ventana temporal en la que el bloqueo es 
efectivo (BlockWindowStart y BlockWindowEnd), así como efectos visuales mediante partículas. 
La función IsBlocked evalúa si un ángulo de ataque está dentro de las regiones válidas para 
bloquear, y retorna los detalles si se bloquea correctamente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackBlock : AttackData
    {
        // Array de regiones angulares que definen desde qué direcciones puede bloquearse un ataque.
        [field: SerializeField] public DirectionalInformation[] BlockDirectionInformation { get; private set; }

        // Tiempo (dentro de la fase de ataque) en el que empieza la ventana de bloqueo.
        [field: SerializeField] public PhaseTime BlockWindowStart { get; private set; }

        // Tiempo en el que termina la ventana de bloqueo.
        [field: SerializeField] public PhaseTime BlockWindowEnd { get; private set; }

        // Prefab de partículas que se instanciarán al bloquear exitosamente.
        [field: SerializeField] public GameObject Particles { get; private set; }

        // Posición relativa donde se instanciarán las partículas.
        [field: SerializeField] public Vector2 ParticlesOffset { get; private set; }


        // Verifica si el ángulo dado está dentro de alguna de las regiones bloqueables. Devuelve `true` si se bloqueó, y también entrega la información de la región que lo bloqueó.
        public bool IsBlocked(float angle, out DirectionalInformation directionalInformation)
        {
            // Inicializa la salida como nula por defecto.
            directionalInformation = null;

            // Itera sobre cada región definida.
            foreach (var directionInformation in BlockDirectionInformation)
            {
                // Comprueba si el ángulo está entre los límites de esta región.
                var blocked = directionInformation.IsAngleBetween(angle);

                if (!blocked)
                    continue;

                // Si el ángulo coincide, asigna la región que bloqueó y retorna true.
                directionalInformation = directionInformation;
                return true;
            }

            // Si ninguna región coincide, retorna false.
            return false;
        }
    }
}
