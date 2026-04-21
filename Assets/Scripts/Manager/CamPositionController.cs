using UnityEngine;

namespace Avocado
{
    public class CamPositionController : MonoBehaviour
    {
        [field: SerializeField] public Transform newPosition { get; private set; }
        [field: SerializeField] public Vector2 activeDirection { get; private set; }
        
     
    }
}
