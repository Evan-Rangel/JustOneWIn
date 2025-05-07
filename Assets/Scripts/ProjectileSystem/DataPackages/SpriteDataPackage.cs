using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
SpriteDataPackage es una clase derivada de ProjectileDataPackage que contiene un único campo: 
un Sprite. Este sprite puede ser asignado por el arma que lanza el proyectil para que se 
muestre visualmente de forma diferente (por ejemplo, flechas de fuego vs. flechas normales). 
Esto permite reutilizar un solo prefab de proyectil, cambiando su aspecto visual según sea 
necesario, sin necesidad de múltiples prefabs.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class SpriteDataPackage : ProjectileDataPackage
    {
        // Sprite a usar por el proyectil, definido por el arma que lo genera.
        // Usamos [field: SerializeField] para permitir la serialización del campo privado y exposición como propiedad pública de solo lectura.
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
