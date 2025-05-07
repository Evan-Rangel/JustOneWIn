using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackOptionalSprite se utiliza para asignar un sprite alternativo a un ataque, si así 
se desea. Esto permite cambiar la apariencia del arma, efecto visual o personaje durante un ataque 
específico sin afectar el sprite base. Es útil para efectos visuales personalizables o 
diferenciación de ataques.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackOptionalSprite : AttackData
    {
        // Indica si se debe usar un sprite alternativo en este ataque.
        // Esto puede ser útil para representar visualmente un estado diferente (por ejemplo, ataque cargado, ataque especial, etc.).
        [field: SerializeField] public bool UseOptionalSprite { get; private set; }

        // El sprite opcional que se usará si 'UseOptionalSprite' es verdadero.
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
