using System;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
   
    public class PlatformPoint : MonoBehaviour
    {
        [SerializeField] bool ActivePlatform;

        public PlatformPoint nextPoint;
        public  PlatformData platformData;
        public bool isLastPoint;
        private void Start()
        {
            platformData.point = this;
        }
        private void Update()
        {
            if (ActivePlatform)
            {
                StartPoint();
                ActivePlatform = false;
            }
        }
        public void StartPoint()
        {
            PlatformMovement platform = GridManager.instance.GetPlatform(transform.position, transform.rotation);
            platform.data = platformData;
        }
        public void FinalPoint(GameObject platform)
        { 
            GridManager.instance.ReturnToPool(platform);
        }
  
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (nextPoint == null)
                return;
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }
}
