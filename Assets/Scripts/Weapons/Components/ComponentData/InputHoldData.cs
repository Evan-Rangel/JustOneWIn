/*---------------------------------------------------------------------------------------------
InputHoldData es una clase que actúa como contenedor de configuración para el componente InputHold. 
En este caso, no necesita datos distintos para cada ataque (por eso no usa el genérico 
ComponentData<T>), sino que se aplica de forma general.
La función principal del script es declarar que depende del componente InputHold
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class InputHoldData : ComponentData
    {
        // Define la dependencia con el componente InputHold.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(InputHold);
        }
    }
}
