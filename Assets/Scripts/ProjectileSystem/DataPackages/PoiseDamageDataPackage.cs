using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
PoiseDamageDataPackage es una clase derivada de ProjectileDataPackage utilizada para transportar 
la información específica del daño al poise (estabilidad o equilibrio) de un objetivo. Este tipo 
de daño puede ser utilizado por otros componentes (como PoiseDamage) para aplicar efectos 
relacionados con desestabilización o interrupción de animaciones al impactar un proyectil.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class PoiseDamageDataPackage : ProjectileDataPackage
    {
        // Cantidad de daño al "poise" (estabilidad) que inflige el proyectil.
        // El atributo SerializeField permite que Unity serialice el campo privado,
        // mientras que el modificador 'private set' impide que sea modificado desde fuera de la clase.
        [field: SerializeField] public float Amount { get; private set; }
    }
}
