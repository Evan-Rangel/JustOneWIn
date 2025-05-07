using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackData actúa como clase base para todas las estructuras de datos que representan 
propiedades de un ataque. Tiene un campo oculto name, usado para identificar cada ataque con 
un nombre como "Attack 1", "Attack 2", etc. El método SetAttackName permite establecer ese 
nombre dinámicamente según un índice.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class AttackData
    {
        // Nombre interno del ataque.
        [SerializeField, HideInInspector] private string name;

        // Asigna un nombre al ataque basado en su índice.
        public void SetAttackName(int i) => name = $"Attack {i}";
    }
}
