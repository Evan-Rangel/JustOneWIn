using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackActionHitBox hereda de AttackData, lo que significa que contiene información 
específica de un ataque en el sistema de armas. En particular, define un Rect (rectángulo) 
llamado HitBox, que representa el área de colisión del ataque, y un booleano Debug que 
probablemente se usa para mostrar visualmente el hitbox en tiempo real durante pruebas o desarrollo.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackActionHitBox : AttackData
    {
        public bool Debug;
        [field: SerializeField] public Rect HitBox { get; private set; }
    }
}