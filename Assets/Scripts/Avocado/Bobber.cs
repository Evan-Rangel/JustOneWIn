using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script hace un movimiento de vaivén suave en Y (arriba y abajo) usando una AnimationCurve,
permite iniciar y detener el movimiento de manera controlada.
Se podría usar para:

-Efectos visuales en armas, power-ups flotando
-Botones animados
-Elementos de HUD que 
---------------------------------------------------------------------------------------------*/

namespace Avocado
{
    public class Bobber : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float yOffset; // Cuánto se va a mover hacia arriba y abajo
        [SerializeField] private float bobDuration; // Cuánto dura un ciclo completo del movimiento
        [SerializeField] private float stopMultiplier; // Qué tan rápido se detiene el "bobbing" al pedir que pare
        [SerializeField] private AnimationCurve bobCurve; // Curva que define cómo se mueve (más natural que un simple seno/coseno)

        private float t; 

        private bool isBobbing; 
        private bool shouldStopBobbing; 

        private Vector3 initialPosition; 
        private Vector3 currentPosition;
        #endregion

        #region Funciones
        public void StartBobbing()
        {
            isBobbing = true; // Empieza a moverse
            shouldStopBobbing = false; // Asegura que no está en modo de detenerse
            t = 0f; // Reinicia el tiempo
        }

        public void StopBobbing()
        {
            shouldStopBobbing = true; // Marca que queremos detener el movimiento
        }

        private void Update()
        {
            if (!isBobbing)
            {
                return; // Si no está activo el bobbing, no hace nada
            }

            // Si estamos deteniéndolo y ya se terminó el tiempo
            if (shouldStopBobbing && t <= 0f)
            {
                isBobbing = false; // Detenemos completamente
                transform.localPosition = initialPosition; // Regresamos a su posición original
                return;
            }

            // Calcula cuánto debe moverse en base a la curva
            var curveValue = bobCurve.Evaluate(t / bobDuration);

            // Actualiza solo la posición Y (vertical)
            currentPosition = transform.localPosition;
            currentPosition.y = initialPosition.y + (yOffset * curveValue);

            transform.localPosition = currentPosition; // Aplica la nueva posición

            // Actualiza el tiempo "t"
            if (!shouldStopBobbing)
            {
                t += Time.deltaTime; // Sigue avanzando normalmente
                t %= bobDuration; // Reinicia cuando llega al final del ciclo 
            }
            else
            {
                // Si estamos deteniendo, hacemos una reversa parcial
                if (t > bobDuration / 2f)
                {
                    t = bobDuration - t; // Si pasa la mitad, invierte para que cierre bonito
                }

                t -= (Time.deltaTime * stopMultiplier);
            }
        }

        private void Awake()
        {
            initialPosition = transform.localPosition; // Guarda donde estaba inicialmente
        }
        #endregion
    }
}
