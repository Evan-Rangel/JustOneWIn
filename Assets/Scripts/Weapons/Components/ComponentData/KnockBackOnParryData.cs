/*---------------------------------------------------------------------------------------------
KnockBackOnParryData es una clase de configuración que define cómo debe comportarse el retroceso 
cuando el personaje realiza un parry (bloqueo perfecto o desvío de ataque). Hereda de 
ComponentData<AttackKnockBack>, lo que permite manejar un conjunto de datos específicos para 
cada ataque relacionado con esta mecánica.
El método SetComponentDependency asegura que esta clase esté vinculada al componente KnockBackOnParry
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class KnockBackOnParryData : ComponentData<AttackKnockBack>
    {
        // Establece el componente que depende de esta clase de datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(KnockBackOnParry);
        }
    }
}
