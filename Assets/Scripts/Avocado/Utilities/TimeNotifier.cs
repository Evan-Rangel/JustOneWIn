using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Lanza un evento (OnNotify) una vez que ha pasado una duración específica. Espara los cooldowns
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    // Temporizador que dispara un evento después de cierto tiempo.
    // Puede configurarse para repetir o ejecutarse solo una vez.
    public class TimeNotifier
    {
        public event Action OnNotify;

        private float duration;
        private float targetTime;
        private bool enabled;

        // Inicializa el temporizador con una duración.
        public void Init(float dur, bool reset = false)
        {
            enabled = true;
            duration = dur;
            SetTargetTime();

            if (reset)
            {
                // Reinicia automáticamente cada vez que se cumple el tiempo
                OnNotify += SetTargetTime;
            }
            else
            {
                // Se desactiva después de una sola ejecución
                OnNotify += Disable;
            }
        }

        // Establece el próximo tiempo objetivo.
        private void SetTargetTime()
        {
            targetTime = Time.time + duration;
        }

        // Desactiva el temporizador.
        public void Disable()
        {
            enabled = false;
            OnNotify -= Disable;
        }

        // Se debe llamar cada frame para verificar si el tiempo ha pasado.
        public void Tick()
        {
            if (!enabled) return;

            if (Time.time >= targetTime)
            {
                OnNotify?.Invoke();
            }
        }
    }
}
