using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
MovementData es una clase que almacena la información relacionada con el movimiento que ocurre 
durante un ataque, como desplazamiento hacia adelante, saltos, empujes, etc. Hereda de 
ComponentData<AttackMovement>, lo que permite definir diferentes configuraciones de movimiento 
por ataque.
El método SetComponentDependency() especifica que esta configuración está destinada a ser usada 
por el componente Movement, que será el encargado de aplicar el movimiento en el juego durante 
una animación de ataque.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class MovementData : ComponentData<AttackMovement>
    {
        // Define el componente dependiente que usará estos datos, en este caso, el componente Movement.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Movement);
        }
    }
}
