using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricEnemyCollision : MonoBehaviour
    {
        public event Action<Collider2D> OnPlayerEnter;
        public event Action<Collider2D> OnPlayerExit;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke(collision);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke(collision);
            }
        }

    }
}
