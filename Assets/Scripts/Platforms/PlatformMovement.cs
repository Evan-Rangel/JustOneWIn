using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace Avocado
{
    
    public class PlatformMovement : MonoBehaviour
    {
        public Transform targetPoint;
        public float speed;
        public bool MoveToPoint()
        {
            if (targetPoint == null) return false;
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * speed);

            return Vector2.Distance(transform.position, targetPoint.position) < 0.01f;
        }
    }
}
