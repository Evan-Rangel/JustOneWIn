/*---------------------------------------------------------------------------------------------
Esta clase ChargeData es parte del sistema modular de componentes de armas. Su propósito es 
definir y contener la información que necesita el componente Charge, probablemente relacionado 
con ataques cargados o acumulativos.
Hereda de ComponentData<AttackCharge>, lo que significa que trabaja con un tipo de datos 
específico (AttackCharge) diseñado para ese tipo de acción.
Al establecer ComponentDependency = typeof(Charge), asegura que este conjunto de datos solo 
sea utilizado junto con el componente Charge.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ChargeData : ComponentData<AttackCharge>
    {
        // Establece la dependencia de componente necesaria para usar estos datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Charge);
        }
    }
}
