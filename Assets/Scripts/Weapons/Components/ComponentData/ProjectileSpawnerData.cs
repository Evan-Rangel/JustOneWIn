/*---------------------------------------------------------------------------------------------
ProjectileSpawnerData define y encapsula los datos necesarios para un componente ProjectileSpawner, 
que es responsable de generar proyectiles cuando se ejecuta un ataque. Al heredar de 
ComponentData<AttackProjectileSpawner>, esta clase puede manejar configuraciones específicas por 
tipo de ataque (como el tipo de proyectil, velocidad, etc.). El método SetComponentDependency() 
asegura que estos datos solo se asocien con armas que efectivamente usan el componente 
ProjectileSpawner.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ProjectileSpawnerData : ComponentData<AttackProjectileSpawner>
    {
        // Asocia esta clase de datos con el componente ProjectileSpawner.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileSpawner);
        }
    }
}
