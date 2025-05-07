/*---------------------------------------------------------------------------------------------
KnockBackData es una clase que representa la configuración de "retroceso" o empuje que se aplica 
al golpear un objetivo. Hereda de ComponentData<AttackKnockBack>, lo que indica que puede tener 
distintos valores de retroceso por cada ataque del arma.
El método SetComponentDependency establece que este conjunto de datos debe ser usado por un 
componente KnockBack
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class KnockBackData : ComponentData<AttackKnockBack>
    {
        // Define qué componente depende de estos datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(KnockBack);
        }
    }
}
