using Avocado.Weapons.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
WeaponSpriteData es una clase de datos que contiene configuraciones para el componente WeaponSprite, 
el cual probablemente maneja los sprites del arma durante las distintas fases de ataque. Hereda 
de ComponentData<AttackSprites>, lo que indica que por cada ataque puede tener una colección distinta 
de sprites (AttackSprites). Al establecer la dependencia con WeaponSprite, se asegura que esta 
clase de datos solo será utilizada si ese componente está presente en el arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class WeaponSpriteData : ComponentData<AttackSprites>
    {
        // Establece que este conjunto de datos depende del componente WeaponSprite.
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(WeaponSprite);
        }
    }
}
