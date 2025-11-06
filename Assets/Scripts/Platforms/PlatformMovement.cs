using UnityEngine;


namespace Avocado
{
    public class PlatformMovement : MonoBehaviour
    {
        public Transform targetPoint;
        public float speed;
        float acceleration;
       float currentSpeed;
       
        public float DistanceToPoint()
        {
           
            if (targetPoint == null) return 0;

            float brakingDistance = (currentSpeed * currentSpeed) / (2f * acceleration);
            if (Vector2.Distance(transform.position, targetPoint.position)<brakingDistance)
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);


   

            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * currentSpeed);

            return Vector2.Distance(transform.position, targetPoint.position);
        }
        public void InitMovement(Transform _newTargetPoint, float _speed, float _acceleration)
        {
            currentSpeed = 0;
            speed = _speed;
            targetPoint = _newTargetPoint;
            acceleration = _acceleration;
        }
    }
}
