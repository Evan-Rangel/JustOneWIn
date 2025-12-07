using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricOctopusPlayerDetector : MonoBehaviour
    {
        FabricOctopusController controller;
        private void Awake()
        {
            controller = GetComponentInParent<FabricOctopusController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                controller.TargetFinded();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                controller.TargetLost();
            }
        }
    }
}
