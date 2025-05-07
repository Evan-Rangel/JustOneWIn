/*---------------------------------------------------------------------------------------------
Este script define la clase DamageOnBlockData, que es un contenedor de datos para el componente 
DamageOnBlock. Hereda de ComponentData<AttackDamage>, lo que significa que almacena un conjunto 
de estructuras AttackDamage, una por cada ataque (o una sola si se repite para todos).
El método SetComponentDependency() declara que esta clase está asociada directamente con el 
componente DamageOnBlock, lo cual permite al sistema saber que debe usar estos datos con ese 
componente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    // Define los datos necesarios para el componente DamageOnBlock.
    public class DamageOnBlockData : ComponentData<AttackDamage>
    {
        // Establece la dependencia del componente correspondiente (DamageOnBlock).
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(DamageOnBlock);
        }
    }
}
