using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricTorretCollision : MonoBehaviour
    {
        FabricTorretCanon canon;
        private void Awake()
        {
            canon= transform.root.GetComponentInChildren<FabricTorretCanon>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //canon.Aiming(collision.transform);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //canon.StopAiming();
            }
        }
    }
}
