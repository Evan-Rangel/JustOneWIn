using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Utilities
{
    /// <summary>
    /// TimeNotifier fires off an event after some duration once the timer has started. The timer can also be configured
    /// to automatically restart the timer once the duration has passed or to only trigger once.
    /// </summary>
    public class TimeNotifier
    {
        /// <summary>
        /// Event will be invoked once duration has passed. If timer is set to restart, it will be invoked every time
        /// the duration passes
        /// </summary>

        public event Action OnNotify;

        private float duration;
        private float targetTime;

        private bool enabled;

        public void Init(float dur, bool reset = false)
        {
            enabled = true;

            duration = dur;
            SetTargetTime();

            if (reset)
            {
                // If reset is true, then when duration has passed automatically calculate new target time
                OnNotify += SetTargetTime;
            }
            else
            {
                // Otherwise, disable when duration has passed
                OnNotify += Disable;
            }
        }

        private void SetTargetTime()
        {
            targetTime = Time.time + duration;
        }

        public void Disable()
        {
            enabled = false;

            OnNotify -= Disable;
        }

        public void Tick()
        {
            if (!enabled)
                return;

            if (Time.time >= targetTime)
            {
                OnNotify?.Invoke();
            }
        }
    }
}
