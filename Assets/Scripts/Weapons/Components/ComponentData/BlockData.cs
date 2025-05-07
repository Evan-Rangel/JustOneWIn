/*---------------------------------------------------------------------------------------------
Esta clase es parte del sistema modular de armas, y está diseñada para contener los datos que 
usará el componente Block, que probablemente se encarga de la lógica de bloqueo o defensa en el combate.
Hereda de ComponentData<AttackBlock>, lo cual la vincula con un tipo específico de ataque defensivo.
Define su dependencia con el componente Block en tiempo de ejecución, asegurando que este bloque 
de datos solo se utilice si el componente correcto está presente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class BlockData : ComponentData<AttackBlock>
    {
        // Método que define qué componente requiere esta clase de datos.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Block);
        }
    }
}
