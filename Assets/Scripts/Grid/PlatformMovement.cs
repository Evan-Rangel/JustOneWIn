using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Avocado
{
    public class PlatformMovement : MonoBehaviour
    {
        public GameObject currentPoint;
        bool isMoving = false;
        bool stopInPoints;
        public void SetStopInPoint(bool value) => stopInPoints = value;
        float speed;
        public void SetSpeed(float value) => speed = value;

        float stopTime;
        public void SetStopTime(float value)=> stopTime = value;

        float movementTime;
        public void SetMovementTime(float value)=> movementTime = value;

        bool reverse;
        public void SetReverse(bool value)=> reverse=value;
        private void Start()
        {
            stopInPoints = GridManager.instance.stopInPoints;
            
            speed = GridManager.instance.platformSpeed;
            stopTime= GridManager.instance.platformStopTime;
            movementTime = GridManager.instance.platformMovementTime;
             isMoving = true;


             StartCoroutine(Stops());
        }
        void Update()
        {
            MoveToPoint();
        }
        void MoveToPoint()
        {
            if (currentPoint == null || !isMoving) { return; }
            PlatformPoint point;
            transform.position = Vector2.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * speed);
            if (Vector2.Distance(transform.position, currentPoint.transform.position) < 0.01f)
            {
                 
                point = currentPoint.GetComponent<PlatformPoint>();
                point.pointEvent.Invoke(gameObject);

                currentPoint =(reverse)?point.prevPosition: point.nextPosition;

                if(stopInPoints)
                StartCoroutine(DelayAction());
            }
          
        }
        IEnumerator DelayAction()
        {
            isMoving = false;
            yield return Helpers.GetWait(stopTime);
            isMoving = true;
        }
        IEnumerator Stops()
        {
            while (true)
            {
                if (stopInPoints) { isMoving = true; yield return new WaitUntil(() => !stopInPoints); }
                Debug.Log("Stops");
                isMoving = false;
                yield return Helpers.GetWait(stopTime);
                isMoving = true;

                yield return Helpers.GetWait(movementTime);
                isMoving = false;
            }
        }
       
    }
}
