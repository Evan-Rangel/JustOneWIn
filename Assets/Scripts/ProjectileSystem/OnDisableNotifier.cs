using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.ProjectileSystem
{
    /// <summary>
    /// This class fires off an event whenever the GameObject it is attached to is disabled or destroyed
    /// </summary>
    public class OnDisableNotifier : MonoBehaviour
    {
        public event Action OnDisableEvent;

        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }

        [ContextMenu("Test")]
        private void Test()
        {
            gameObject.SetActive(false);
        }
    }
}
