/*---------------------------------------------------------------------------------------------
DamageOnHitBoxActionData es una clase que define los datos que necesita el componente 
DamageOnHitBoxAction para funcionar correctamente. Hereda de ComponentData<AttackDamage>, 
lo cual significa que puede contener múltiples configuraciones de daño (AttackDamage), una 
por cada ataque o una que se repite si está configurado así.
El método SetComponentDependency() se usa para que el sistema reconozca automáticamente que 
estos datos pertenecen al componente DamageOnHitBoxAction.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DamageOnHitBoxActionData : ComponentData<AttackDamage>
    {
        // Establece que esta clase de datos depende del componente DamageOnHitBoxAction.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(DamageOnHitBoxAction);
        }
    }
}
