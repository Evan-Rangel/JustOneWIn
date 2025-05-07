using Avocado.Weapons.Components; 
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define un delegado llamado ConditionalDelegate, el cual representa un método que recibe:
-Un Transform (usualmente del atacante o fuente del daño),
-Y devuelve un bool indicando si el ataque fue bloqueado exitosamente,
-Además, proporciona (vía out) información específica sobre la dirección del bloqueo 
(DirectionalInformation), como cuánta absorción de daño, empuje o poise aplica.
Este delegado se utiliza en otros scripts, como los modificadores (BlockKnockBackModifier, 
BlockPoiseDamageModifier), para aplicar efectos de mitigación cuando un ataque es bloqueado.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Modifiers
{
    // Define un delegado que representa una función que determina si un ataque fue bloqueado,
    // dado el transform del atacante (source), y que devuelve información direccional del bloqueo si fue exitoso.
    public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}
