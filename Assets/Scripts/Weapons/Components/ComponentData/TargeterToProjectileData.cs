/*---------------------------------------------------------------------------------------------
TargeterToProjectileData es una clase de configuración que indica que está asociada con el componente 
TargeterToProjectile. A diferencia de otros ComponentData<T>, esta clase no requiere datos específicos 
por ataque, por lo tanto hereda directamente de ComponentData. Su único propósito es declarar la 
dependencia con el componente lógico TargeterToProjectile, que probablemente transfiere información 
de objetivos hacia los proyectiles cuando son generados.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class TargeterToProjectileData : ComponentData
    {
        // Especifica que este conjunto de datos está vinculado al componente TargeterToProjectile.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(TargeterToProjectile);
        }
    }
}
