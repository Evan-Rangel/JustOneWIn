using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricDronPlayerDetector : MonoBehaviour
    {
        FabricDronController Controller;
        private void Awake()
        {
            Controller = GetComponentInParent<FabricDronController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Controller.TargetFinded(collision.transform);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Controller.TargetLost();
            }
        }
    }
}
