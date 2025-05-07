using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackParry define los datos necesarios para ejecutar un parry, que es una técnica 
defensiva que cancela o desvía un ataque enemigo si se realiza en el momento y dirección correctos. 
Se especifican las regiones angulares donde el parry es efectivo (ParryDirectionalInformation), 
el tiempo en la fase de ataque en que el parry está activo (ParryWindowStart y End), así como 
partículas visuales para dar retroalimentación al jugador. La función IsBlocked se usa para 
determinar si un ataque entrante puede ser parado basándose en su ángulo.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackParry : AttackData
    {
        // Regiones direccionales donde el parry puede bloquear un ataque (por ejemplo, solo de frente).
        [field: SerializeField] public DirectionalInformation[] ParryDirectionalInformation { get; private set; }

        // Momento en que comienza la ventana de parry, dentro de la fase del ataque.
        [field: SerializeField] public PhaseTime ParryWindowStart { get; private set; }

        // Momento en que termina la ventana de parry.
        [field: SerializeField] public PhaseTime ParryWindowEnd { get; private set; }

        // Prefab de partículas que se instancian cuando se realiza un parry exitoso.
        [field: SerializeField] public GameObject Particles { get; private set; }

        // Offset de las partículas respecto al origen (normalmente el jugador o arma).
        [field: SerializeField] public Vector2 ParticlesOffset { get; private set; }

        // Método que evalúa si el ángulo de un ataque entrante está dentro de alguna región válida de parry.
        public bool IsBlocked(float angle, out DirectionalInformation directionalInformation)
        {
            directionalInformation = null;

            foreach (var directionInformation in ParryDirectionalInformation)
            {
                var blocked = directionInformation.IsAngleBetween(angle);

                if (!blocked)
                    continue;

                directionalInformation = directionInformation;
                return true;
            }

            return false;
        }
    }
}
