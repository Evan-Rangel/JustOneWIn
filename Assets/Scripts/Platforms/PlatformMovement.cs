using UnityEngine;


namespace Avocado
{
    public class PlatformMovement : MonoBehaviour
    {
        [SerializeField] Transform[] sensors;
   
        [field: SerializeField] public PlatformPoint targetPoint { get; private set; }
        float speed;
        float acceleration;
        float currentSpeed;
        float distanceToSpawnDeathZone;
        [SerializeField]GameObject collision;
        [SerializeField] GameObject[] deathZones;
       
        public void DisableCollider()
        { 
            collision.SetActive(false); 
        }
        public void EnableCollider()
        {
            collision.SetActive(true);
        }
        private void OnDisable()
        {
            collision.SetActive(true);
            for (int i = 0; i < deathZones.Length; i++)
            {
                deathZones[i].SetActive(false);
            }
        }
        public void ActiveDeathZone(int deathZoneIndex)
        {
            Debug.Log("aaaa");

            if (deathZones.Length > deathZoneIndex)
            {
                Debug.Log("Active death zone");
                deathZones[deathZoneIndex].SetActive(true);
            }
        }
        public void DisableDeathZone(int deathZoneIndex)
        {
            if (deathZones.Length > deathZoneIndex)
                deathZones[deathZoneIndex].SetActive(false);
        }

        public void DistanceCollisionToPoint(int _deathZoneIndex)
        {

            if (targetPoint == null ) return;
            if (Vector2.Distance(sensors[_deathZoneIndex].position, targetPoint.transform.position)<distanceToSpawnDeathZone)
                deathZones[_deathZoneIndex].SetActive(true);
        }
        public float DistanceToPoint()
        {
            if (targetPoint == null) return 0;


            float brakingDistance = (currentSpeed * currentSpeed) / (2f * acceleration);
            float currentDistance = Vector2.Distance(transform.position, targetPoint.transform.position);
            

            if (currentDistance<brakingDistance)
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);

            transform.position = Vector2.MoveTowards(transform.position, targetPoint.transform.position, Time.deltaTime * currentSpeed);


            currentDistance=Vector2.Distance(transform.position, targetPoint.transform.position);
            return currentDistance;
        }
        public void InitMovement(PlatformPoint _newTargetPoint, float _speed, float _acceleration, float _distanceToSpawnDeathZone)
        {
            currentSpeed = 0;
            speed = _speed;
            targetPoint = _newTargetPoint;
            acceleration = _acceleration;
            distanceToSpawnDeathZone = _distanceToSpawnDeathZone;
        }
        public void SetNextTarget(PlatformPoint _newTargetPoint)
        {
            targetPoint = _newTargetPoint;
        }
        
    }
}
