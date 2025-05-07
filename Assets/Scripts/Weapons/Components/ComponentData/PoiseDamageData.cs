/*---------------------------------------------------------------------------------------------
La clase PoiseDamageData define los datos necesarios para aplicar daño a la estabilidad (poise) 
de un objetivo durante un ataque. Hereda de ComponentData<AttackPoiseDamage>, lo que le permite 
tener diferentes configuraciones de daño por ataque. El método SetComponentDependency() vincula 
estos datos al componente PoiseDamage, garantizando que solo se apliquen si ese comportamiento 
existe en el arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class PoiseDamageData : ComponentData<AttackPoiseDamage>
    {
        // Establece la dependencia de este conjunto de datos con el componente PoiseDamage.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(PoiseDamage);
        }
    }
}
