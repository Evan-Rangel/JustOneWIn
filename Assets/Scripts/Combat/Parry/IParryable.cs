/*---------------------------------------------------------------------------------------------
Este script de Interfaz que define que un objeto puede ser afectado por una acción de Parry.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.Parry
{
    public interface IParryable
    {
        // Ejecuta la lógica del parry usando la información proporcionada.
        void Parry(ParryData data);
    }
}
