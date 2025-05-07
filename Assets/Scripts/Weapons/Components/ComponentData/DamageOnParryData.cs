/*---------------------------------------------------------------------------------------------
DamageOnParryData es una clase de datos especializada para el componente DamageOnParry. Define 
los valores de daño (AttackDamage) que se aplicarán cuando un parry (contraataque o desvío) 
ocurra durante un ataque.
Gracias a que hereda de ComponentData<AttackDamage>, puede manejar múltiples configuraciones 
de daño, una por cada ataque configurado en el sistema de armas. El método SetComponentDependency() 
le indica al sistema que esta clase de datos está vinculada específicamente al componente DamageOnParry
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DamageOnParryData : ComponentData<AttackDamage>
    {
        // Establece la dependencia del componente.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(DamageOnParry);
        }
    }
}
