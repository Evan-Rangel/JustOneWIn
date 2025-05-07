/*---------------------------------------------------------------------------------------------
DrawData es una clase que define los datos específicos para el componente Draw, que probablemente 
maneja una mecánica de "preparación" o "carga" visual de un arma (como dibujar un arco antes de disparar).
Al heredar de ComponentData<AttackDraw>, permite almacenar información personalizada por ataque 
relacionada con ese comportamiento. El método SetComponentDependency() le dice al sistema que 
esta configuración debe ser usada junto con el componente Draw
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DrawData : ComponentData<AttackDraw>
    {
        // Establece la dependencia del componente correspondiente a estos datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Draw);
        }
    }
}
