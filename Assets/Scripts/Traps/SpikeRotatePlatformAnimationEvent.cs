using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class SpikeRotatePlatformAnimationEvent : MonoBehaviour
    {
        public event Action onActivePlatform;
        public event Action onDeactivatePlatform;

        void ActivatePlatform()
        {
            onActivePlatform?.Invoke();
        }
        void DeactivatePlatform()
        {
            onDeactivatePlatform?.Invoke();    
        }
    }
}
