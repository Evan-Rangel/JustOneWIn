using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricSawBallCollision : MonoBehaviour
    {
        [SerializeField] FabricSawBallController fabricDronController;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                fabricDronController.PlayerIsClose();
            }
        }
    }
}
