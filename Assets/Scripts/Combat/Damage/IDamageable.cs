
/*---------------------------------------------------------------------------------------------
Este script es una interfaz, lo que significa que no implementa lógica, sino que obliga a 
las clases que la usen a implementar el método Damage.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.Damage
{
    // Interfaz que define un comportamiento de daño. 
    // Cualquier clase que implemente esto debe definir cómo recibe daño.

    public interface IDamageable
    {
        // Aplica daño al objeto.
        void Damage(DamageData data);
    }
}

