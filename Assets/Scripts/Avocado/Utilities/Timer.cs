using System;
using UnityEngine;

namespace Avocado.Utilities
{
    public class Timer : MonoBehaviour
    {
        #region Events
        //public event Action OnNotify;
        public event Action OnTimerDone;
        #endregion

        #region Floats
        private float startTime;
        private float duration;
        private float targetTime;
        #endregion

        #region Flags
        private bool isActive;
        private bool eneable;
        #endregion

        #region Construct
        public Timer(float duration)
        {
            this.duration = duration;
        }
        #endregion

        #region Functions
        public void StartTimer()
        { 
            startTime = Time.time;
            targetTime = startTime + duration;
            isActive = true;
        }

        public void StopTimer()
        {
            isActive = false;
        }

        public void Tick()
        {
            if (!isActive)
            {
                return;
            }
            if (Time.time >= targetTime)
            {
                OnTimerDone?.Invoke();
                StopTimer();
            }
        }
  
        /*
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
        */
        #endregion
    }
}
