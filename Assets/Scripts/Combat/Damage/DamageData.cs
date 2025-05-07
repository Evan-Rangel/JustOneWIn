using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es una clase que contiene:
-Amount: cuánta cantidad de daño se quiere aplicar.
-Source: quién o qué está causando ese daño (el jugador, un enemigo, una trampa, etc.).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Combat.Damage
{
    // Contiene la información que se transmite a través de IDamageable.
    public class DamageData
    {
        // Cantidad de daño a aplicar
        public float Amount { get; private set; }

        // Objeto que origina el daño
        public GameObject Source { get; private set; }

        // Crea una nueva instancia con daño y fuente.
        public DamageData(float amount, GameObject source)
        {
            Amount = amount;
            Source = source;
        }

        // Permite modificar el daño después de haber creado el objeto.
        public void SetAmount(float amount)
        {
            Amount = amount;
        }
    }
}
