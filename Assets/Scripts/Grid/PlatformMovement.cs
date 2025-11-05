using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace Avocado
{
    [Serializable]
    public class PlatformData { public int speed; public bool stopInPoints; public float stopTime; public float movementTime; public PlatformPoint point; }
    public class PlatformMovement : MonoBehaviour
    {
        public UnityEvent platformEvent;
        bool isMoving = false;

        public PlatformData data;

        IEnumerator delayAction;
        private void OnEnable()
        {
            delayAction = DelayAction();
            isMoving = true;
            StartCoroutine(Stops());
        }
        private void OnDisable()
        {
            isMoving = false;
        }
        void Update()
        {
            if (!isMoving) return;
 
            MoveToPoint();
        }
        void MoveToPoint()
        {
            if (data.point == null) return; 

            transform.position = Vector2.MoveTowards(transform.position, data.point.nextPoint.transform.position, Time.deltaTime * data.speed);

            if (Vector2.Distance(transform.position, data.point.nextPoint.transform.position) < 0.01f)
            {
                data = data.point.nextPoint.platformData;


                if(data.stopInPoints)
                StartCoroutine(delayAction);
            }
          
        }
        IEnumerator DelayAction()
        {
            isMoving = false;
            yield return Helpers.GetWait(data.stopTime);
            isMoving = true;
            StopCoroutine(delayAction);
        }
        IEnumerator Stops()
        {
            while (true)
            {
                if (data.stopInPoints) { isMoving = true; yield return new WaitUntil(() => !data.stopInPoints); }
                isMoving = false;

                yield return Helpers.GetWait(data.stopTime);
                isMoving = true;

                yield return Helpers.GetWait(data.movementTime);
                isMoving = false;
            }
        }
       
    }
}
