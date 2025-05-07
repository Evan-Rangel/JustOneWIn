using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
DamageDataPackage es una clase que representa la cantidad de daño que un proyectil puede causar. 
Hereda de ProjectileDataPackage, lo que la convierte en parte del sistema de paquetes de datos 
que los proyectiles pueden transportar. Esto permite que los distintos componentes accedan a 
datos específicos (como el daño, fuerza, empuje, etc.) de forma flexible y extensible. Se 
utiliza especialmente en sistemas donde múltiples tipos de datos pueden ser procesados 
dinámicamente por distintos componentes del juego.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class DamageDataPackage : ProjectileDataPackage
    {
        // Propiedad serializada que indica cuántos puntos de daño inflige el proyectil.
        [field: SerializeField] public float Amount { get; private set; }
    }
}
