using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script monitorea la distancia entre una posición fija (referencePos) y una posición 
actual (que tú le vas pasando). Cuando la distancia supera (o queda por debajo de) cierto 
valor, lanza un evento (OnNotify). Y lo mejor: puede configurarse para que notifique una sola 
vez o continuamente mientras se cumpla la condición.

Este DistanceNotifier es una clase útil cuando quieres ejecutar una acción automáticamente 
cuando algo se aleja o se acerca a cierta distancia de un punto en el mundo. 
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    // DistanceNotifier detecta si un objeto alcanza cierta distancia desde un punto de referencia y lanza un evento cuando ocurre.
    public class DistanceNotifier
    {
        public event Action OnNotify; 
        private Vector3 referencePos;    
        private float sqrDistance;       
        private bool enabled;           
        private Action<float> notifierCondition; // Acción que guarda la lógica de chequeo (dentro o fuera de la distancia)

        // Inicializa el sistema con una posición de referencia y distancia objetivo.
        public void Init(Vector3 referencePos, float distance, bool checkInside = false, bool triggerContinuously = false)
        {
            this.referencePos = referencePos;
            sqrDistance = distance * distance; // Guardamos distancia al cuadrado

            // Elegimos la función que usaremos al hacer el chequeo
            notifierCondition = checkInside ? CheckInside : CheckOutside;

            enabled = true;

            if (!triggerContinuously)
            {
                // Si solo debe disparar una vez, agregamos un método para desactivar el sistema después de que se dispare
                OnNotify += Disable;
            }
        }

        // Desactiva las notificaciones
        public void Disable()
        {
            enabled = false;
            OnNotify -= Disable;
        }

        // Llama a este método en Update o FixedUpdate con la posición actual para hacer el chequeo.
        public void Tick(Vector3 pos)
        {
            if (!enabled) return;

            float currentSqrDistance = (referencePos - pos).sqrMagnitude;
            notifierCondition.Invoke(currentSqrDistance); // Llama a la función elegida (CheckInside o CheckOutside)
        }

        private void CheckInside(float dist)
        {
            if (dist <= sqrDistance)
            {
                OnNotify?.Invoke();
            }
        }

        private void CheckOutside(float dist)
        {
            if (dist >= sqrDistance)
            {
                OnNotify?.Invoke();
            }
        }
    }
}
