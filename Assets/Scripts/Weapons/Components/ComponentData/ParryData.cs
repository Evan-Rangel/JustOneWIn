using UnityEngine;

/*---------------------------------------------------------------------------------------------
ParryData es una clase que define los datos específicos que un arma necesita para ejecutar una 
mecánica de "parry" o desvío. Hereda de ComponentData<AttackParry>, lo que significa que espera 
un objeto de datos AttackParry por cada ataque (o uno solo si repeatData está activo). El método 
SetComponentDependency() asegura que esta clase solo se utilice si el componente Parry está 
presente en el arma, lo cual mantiene la arquitectura modular y segura.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ParryData : ComponentData<AttackParry>
    {
        // Este método establece la dependencia con el componente Parry,
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Parry);
        }
    }
}
