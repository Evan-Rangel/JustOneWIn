using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este Script es un componente que es muy útil para sistemas que necesitan saber cuándo un objeto ha sido 
desactivado o destruido, como:
-Proyectiles que deben limpiar referencias cuando desaparecen.
-Sistemas de pooling, donde quieres liberar un objeto al ser desactivado.
-Eventos de muerte o destrucción en enemigos, objetos o efectos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem
{
    public class OnDisableNotifier : MonoBehaviour
    {
        // Evento que puede ser escuchado por otros scripts para saber cuándo el objeto se desactiva
        public event Action OnDisableEvent;

        // Unity llama automáticamente a este método cuando el GameObject se desactiva o destruye
        private void OnDisable()
        {
            // Lanza el evento si hay algún suscriptor
            OnDisableEvent?.Invoke();
        }

        // Método de prueba que se puede ejecutar desde el menú contextual en el editor
        [ContextMenu("Test")]
        private void Test()
        {
            // Desactiva el GameObject manualmente para probar el evento
            gameObject.SetActive(false);
        }
    }
}
