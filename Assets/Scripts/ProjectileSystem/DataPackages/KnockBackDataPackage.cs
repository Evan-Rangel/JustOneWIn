using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
KnockBackDataPackage es un contenedor de datos que forma parte del sistema de proyectiles y 
hereda de ProjectileDataPackage. Su propósito es almacenar información relacionada con el 
retroceso o empuje (knockback) que un proyectil debe aplicar al impactar. Contiene dos datos 
importantes:
-Strength: la magnitud del empuje.
-Angle: la dirección en que se aplicará ese empuje, como un vector 2D.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class KnockBackDataPackage : ProjectileDataPackage
    {
        // Fuerza del empuje o retroceso que se aplicará al impactar un proyectil.
        [field: SerializeField] public float Strength;

        // Dirección en la que se aplicará el empuje, representada como un vector 2D.
        [field: SerializeField] public Vector2 Angle;
    }
}
