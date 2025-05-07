using Avocado.Combat.KnockBack; 
using Avocado.ModifierSystem;   

/*---------------------------------------------------------------------------------------------
Este script es un modificador de retroceso (Modifier<KnockBackData>) que se aplica cuando un 
ataque es bloqueado. Utiliza un delegate llamado ConditionalDelegate para determinar si se 
produjo un bloqueo y recuperar información direccional asociada.
Si el bloqueo es exitoso, la fuerza del retroceso se reduce proporcionalmente a la propiedad 
KnockBackAbsorption de la dirección que bloqueó el ataque, simulando así una defensa más realista.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Modifiers
{
    public class BlockKnockBackModifier : Modifier<KnockBackData>
    {
        // Delegate que decide si el ataque fue bloqueado y devuelve información de la dirección del bloqueo
        private readonly ConditionalDelegate isBlocked;

        public BlockKnockBackModifier(ConditionalDelegate isBlocked)
        {
            this.isBlocked = isBlocked;
        }

        // Modifica el valor del knockback según la absorción del bloqueo
        public override KnockBackData ModifyValue(KnockBackData value)
        {
            // Si el ataque fue bloqueado, accede a la información direccional del bloqueo
            if (isBlocked(value.Source.transform, out var blockDirectionInformation))
            {
                // Reduce la fuerza del knockback en proporción a la absorción configurada
                value.Strength *= (1 - blockDirectionInformation.KnockBackAbsorption);
            }

            // Devuelve los datos modificados
            return value;
        }
    }
}
