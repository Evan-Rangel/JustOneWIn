using Avocado.Combat.PoiseDamage; 
using Avocado.ModifierSystem;    

/*---------------------------------------------------------------------------------------------
Este script define un modificador para el daño de postura (PoiseDamageData) que se aplica cuando 
un ataque es bloqueado exitosamente. Utiliza un ConditionalDelegate para determinar si el ataque 
fue bloqueado, y si lo fue, aplica una reducción al daño basándose en el valor PoiseDamageAbsorption 
de la dirección del bloqueo.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Modifiers
{
    public class BlockPoiseDamageModifier : Modifier<PoiseDamageData>
    {
        // Delegate que determina si el ataque fue bloqueado, y proporciona información direccional si lo fue
        private readonly ConditionalDelegate isBlocked;

        public BlockPoiseDamageModifier(ConditionalDelegate isBlocked)
        {
            this.isBlocked = isBlocked;
        }

        // Método que modifica el valor del daño a la postura
        public override PoiseDamageData ModifyValue(PoiseDamageData value)
        {
            // Si el ataque fue bloqueado, se accede a la información de dirección
            if (isBlocked(value.Source.transform, out var blockDirectionInformation))
            {
                // Se reduce el daño a la postura según el porcentaje de absorción definido
                value.SetAmount(value.Amount * (1 - blockDirectionInformation.PoiseDamageAbsorption));
            }

            // Devuelve los datos (modificados o no)
            return value;
        }
    }
}
