using UnityEngine;

namespace Avocado
{
   
    public class PlatformPoint : MonoBehaviour
    {

        public Transform nextPoint;
        public bool isLastPoint;
        public float timeToReturnToPool;

        [field:SerializeField]public bool spawnPlatformOnPoint { get;private set;  }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (nextPoint == null)
                return;
            Gizmos.DrawLine(transform.position, nextPoint.position);
        }
    }
}
