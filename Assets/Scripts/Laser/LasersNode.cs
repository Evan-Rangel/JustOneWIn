using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class LasersNode : MonoBehaviour
    {
        [SerializeField] LaserController[] lasers;
        [SerializeField, Range(0, 10)] float timeToActivateNextNode;
        [SerializeField] LasersNode nextNode;

        private void Awake()
        {
            lasers = GetComponentsInChildren<LaserController>();
        }
        public void ActivateLasers()
        {
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].ActiveLaser();
            }
            StartCoroutine(ActivateNextNode());
        }
        IEnumerator ActivateNextNode()
        { 
            yield return Helpers.GetWait(timeToActivateNextNode);

            if (nextNode != null)
                nextNode.ActivateLasers();
        }


    }
}
