/*---------------------------------------------------------------------------------------------
Este script Interfaz para objetos que pueden recibir daño de poise (temporalmente ser aturdidos).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.PoiseDamage
{
    public interface IPoiseDamageable
    {
        // Aplica daño de poise al objeto.
        void DamagePoise(PoiseDamageData data);
    }
}
