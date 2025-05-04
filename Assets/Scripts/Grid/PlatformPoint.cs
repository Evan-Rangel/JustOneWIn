using System;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
    public class PlatformPoint : MonoBehaviour
    {
        public UnityEvent<GameObject> pointEvent;
        public  GameObject nextPosition;
        public  GameObject prevPosition;
       
        [field: SerializeField, Range(0.01f, 10)] public float newPlatformSpeed { private set; get; }
        [field: SerializeField, Range(0.01f, 10)] public float newPlatformStopTime { private set; get; }
        [field: SerializeField, Range(0.01f, 10)] public float newPlatformMovementTime { private set; get; }
        [field: SerializeField] public bool newPlatformStopInPoint { private set; get; }
        [field: SerializeField] public bool newPlatformReverse { private set; get; }
        [SerializeField] bool ActivePlatform;
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

            GameObject platform = GridManager.instance.GetPlatform(transform.position, transform.rotation);
           // platform.SetActive(true);
            platform.GetComponent<PlatformMovement>().currentPoint = gameObject;
        }
        public void FinalPoint(GameObject platform)
        { 
            GridManager.instance.ReturnToPool(platform);
        }
        public void ChangeMovementTime(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetMovementTime(newPlatformMovementTime);
        } 
        public void ChangeStopTime(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetStopTime(newPlatformStopTime);
        } 
        public void ChangePlatformSpeed(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetSpeed(newPlatformSpeed);
        }public void ChangePlatformStopInPoint(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetStopInPoint(newPlatformStopInPoint);
        }public void ChangePlatformReverse(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetStopInPoint(newPlatformReverse);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (nextPosition == null)
                return;
            Gizmos.DrawLine(transform.position, nextPosition.transform.position);
        }
    }
}
