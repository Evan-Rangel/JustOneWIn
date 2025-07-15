using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class TeleportController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                
            }
        }
    }
}
