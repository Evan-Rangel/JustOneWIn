using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script clase que contiene los datos necesarios para aplicar daño de poise a un objeto.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.PoiseDamage
{
    public class PoiseDamageData
    {
        /// <summary>
        /// Cantidad de daño de poise a aplicar.
        /// </summary>
        public float Amount { get; private set; }

        /// <summary>
        /// Fuente del daño de poise (quién lo causó).
        /// </summary>
        public GameObject Source { get; private set; }

        /// <summary>
        /// Constructor para inicializar el daño de poise con su cantidad y fuente.
        /// </summary>
        /// <param name="amount">Cantidad de daño de poise.</param>
        /// <param name="source">GameObject que causó el daño.</param>
        public PoiseDamageData(float amount, GameObject source)
        {
            Amount = amount;
            Source = source;
        }

        /// <summary>
        /// Permite cambiar la cantidad de daño de poise después de creado.
        /// </summary>
        /// <param name="amount">Nueva cantidad de daño.</param>
        public void SetAmount(float amount) => Amount = amount;
    }
}
