using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricTorretCollision : MonoBehaviour
    {
        FabricTorretController controller;
        private void Awake()
        {
            controller = GetComponentInParent<FabricTorretController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                controller.Aiming(collision.transform);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {

            }
        }
    }
}
