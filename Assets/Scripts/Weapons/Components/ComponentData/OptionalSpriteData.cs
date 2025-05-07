/*---------------------------------------------------------------------------------------------
OptionalSpriteData es una clase que sirve como contenedor de datos para el componente OptionalSprite. 
Este tipo de datos (AttackOptionalSprite) probablemente contiene información como sprites alternativos 
que pueden mostrarse durante un ataque (por ejemplo, efectos visuales u opciones de personalización 
estética). El método SetComponentDependency() vincula esta clase con el componente OptionalSprite, 
asegurando que los datos se apliquen en el contexto correcto.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class OptionalSpriteData : ComponentData<AttackOptionalSprite>
    {
        // Establece que este conjunto de datos debe ser utilizado por el componente OptionalSprite.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(OptionalSprite);
        }
    }
}
