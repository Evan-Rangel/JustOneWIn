using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
   
    public class PlatformPoint : MonoBehaviour
    {
        [SerializeField] bool ActivePlatform;

        public Transform nextPoint;
        public bool isLastPoint;
        public float timeToReturnToPool;
       public PlatformPointsManager manager;
        private void Start()
        {
            manager = transform.parent.GetComponent<PlatformPointsManager>();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (nextPoint == null)
                return;
            Gizmos.DrawLine(transform.position, nextPoint.position);
        }
    }
}
