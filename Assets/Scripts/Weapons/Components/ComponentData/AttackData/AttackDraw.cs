using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase AttackDraw contiene los datos necesarios para representar el proceso de "carga" de un 
arma, como un arco. Usa una AnimationCurve (DrawCurve) para controlar visual y funcionalmente 
cómo se comporta la carga en el tiempo. El campo DrawTime define cuánto tarda en completarse 
la carga por completo. Esta estructura permite crear ataques más expresivos o realistas que 
dependen del tiempo de preparación antes de ser ejecutados.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class AttackDraw : AttackData
    {
        // Curva que define cómo progresa el tiempo de "tensión" del arma desde 0 a 1.
        [field: SerializeField] public AnimationCurve DrawCurve { get; private set; }

        // Tiempo total que toma realizar la acción de "tensar completamente" el arma.
        // Puedes calcularlo con: drawTime = (1 / tasaDeMuestreo) * númeroDeFotogramas.
        [field: SerializeField] public float DrawTime { get; private set; }
    }
}
