/*---------------------------------------------------------------------------------------------
Esta clase ChargeToProjectileSpawnerData sirve para almacenar los datos necesarios por el 
componente ChargeToProjectileSpawner, que probablemente combina mecánicas de carga (Charge) 
con la creación de proyectiles (ProjectileSpawner).
Hereda de ComponentData<AttackChargeToProjectileSpawner>, lo que indica que trabaja con un 
tipo de ataque personalizado para este comportamiento.
Al definir la dependencia con ChargeToProjectileSpawner, asegura que este conjunto de datos 
no sea utilizado por error con otro tipo de componente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ChargeToProjectileSpawnerData : ComponentData<AttackChargeToProjectileSpawner>
    {
        // Define la dependencia del componente que requiere esta clase de datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ChargeToProjectileSpawner);
        }
    }
}
