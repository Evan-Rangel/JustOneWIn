using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
DrawModifierDataPackage es un paquete de datos usado para almacenar un porcentaje de carga 
(por ejemplo, la fuerza con la que se lanza una flecha o dispara un proyectil). Este valor 
(DrawPercentage) siempre se mantiene entre 0 (sin carga) y 1 (carga máxima), gracias al uso 
de Mathf.Clamp01. Hereda de ProjectileDataPackage, por lo que puede ser enviado y recibido 
por componentes del sistema de proyectiles que estén interesados en modificar el comportamiento 
del disparo según la carga.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class DrawModifierDataPackage : ProjectileDataPackage
    {
        // Propiedad pública para acceder y modificar el porcentaje de "draw" (carga o potencia), asegurando que siempre esté entre 0 y 1.
        public float DrawPercentage
        {
            get => drawPercentage;
            set => drawPercentage = Mathf.Clamp01(value); // Clamp01 restringe el valor entre 0 y 1.
        }

        // Campo privado que almacena el valor real del porcentaje de carga.
        private float drawPercentage;
    }
}
