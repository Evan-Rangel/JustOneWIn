using System;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
TargetsDataPackage es una subclase de ProjectileDataPackage que contiene una lista de Transform, 
los cuales representan los objetivos detectados por el arma que dispara el proyectil. Esto es 
útil para proyectiles con comportamiento inteligente, como misiles teledirigidos o hechizos de 
seguimiento, permitiendo que accedan a sus posibles blancos desde el momento en que son 
instanciados.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    [Serializable]
    public class TargetsDataPackage : ProjectileDataPackage
    {
        // Lista de objetivos (transforms) detectados por un componente "Targeter" del arma.
        // Esta lista puede ser usada por el proyectil para buscar, perseguir o priorizar objetivos.
        public List<Transform> targets;
    }
}
