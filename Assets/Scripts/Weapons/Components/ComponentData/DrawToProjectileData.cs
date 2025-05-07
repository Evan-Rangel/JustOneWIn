/*---------------------------------------------------------------------------------------------
DrawToProjectileData es una clase de datos que indica que existe una relación entre este conjunto 
de datos y el componente DrawToProjectile. No contiene configuraciones por ataque (por eso hereda
de ComponentData y no de ComponentData<T>), lo que sugiere que sus datos son compartidos o que su 
comportamiento no requiere ajustes individuales para cada ataque.
Su función principal es establecer la dependencia con DrawToProjectile
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DrawToProjectileData : ComponentData
    {
        // Establece la dependencia del componente correspondiente.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(DrawToProjectile);
        }
    }
}
