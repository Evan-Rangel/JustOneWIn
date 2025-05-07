using UnityEngine;

/*---------------------------------------------------------------------------------------------
Esta clase es una estructura de datos que forma parte del sistema modular de armas. Sirve para 
alimentar al componente ActionHitBox con información específica. En particular:
Define las capas detectables mediante un LayerMask, lo que permite al sistema saber qué objetos 
pueden ser golpeados o afectados por el HitBox.
Informa al sistema que este bloque de datos debe usarse junto al componente ActionHitBox, 
gracias al método SetComponentDependency().
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class ActionHitBoxData : ComponentData<AttackActionHitBox>
    {
        // Define las capas que este HitBox puede detectar (enemigos, objetos rompibles, etc.)
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

        // Indica qué componente requiere este objeto de datos para funcionar.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ActionHitBox);
        }
    }
}
