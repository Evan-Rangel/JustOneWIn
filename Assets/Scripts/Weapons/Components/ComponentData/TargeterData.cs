/*---------------------------------------------------------------------------------------------
TargeterData define la configuración de datos para el componente Targeter, el cual probablemente 
se encarga de identificar y seleccionar objetivos cuando se ejecuta un ataque. Heredando de 
ComponentData<AttackTargeter>, esta clase maneja datos específicos del comportamiento del targeting 
por cada tipo de ataque. El método SetComponentDependency() asegura que este conjunto de datos 
se vincule exclusivamente a instancias del componente Targeter.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class TargeterData : ComponentData<AttackTargeter>
    {
        // Establece la dependencia del componente para que el sistema sepa que estos datos están asociados con el componente Targeter.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Targeter);
        }
    }
}
