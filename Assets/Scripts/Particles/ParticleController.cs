using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script destruye su propio GameObject cuando se llama al método FinishAnim(). Se espera 
que FinishAnim() se llame al final de una animación mediante un Animation Event en Unity.
---------------------------------------------------------------------------------------------*/

public class ParticleController : MonoBehaviour
{
    // Función que debe llamarse (normalmente desde un Animation Event) al final de la animación
    private void FinishAnim()
    {
        // Destruye el GameObject asociado una vez que termina su animación
        Destroy(gameObject);
    }
}
