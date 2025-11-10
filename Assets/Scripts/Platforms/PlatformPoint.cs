using UnityEngine;

namespace Avocado
{
   
    public class PlatformPoint : MonoBehaviour
    {
        public Transform nextPoint;
        [field:SerializeField]public bool spawnPlatformOnPoint { get;private set;  }
        [field:SerializeField]public int deathZoneIndex { get;private set;  }
        [field:SerializeField]public bool stopPlatformOnPoint { get;private set;  }
        [field:SerializeField]public bool platformDeathPoint { get;private set;  }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (nextPoint == null)
                return;
            Gizmos.DrawLine(transform.position, nextPoint.position);
        }
    }
}
