using UnityEngine;

namespace Avocado
{
    public class PlatformSense : MonoBehaviour
    {
        Transform platform;
        Vector2 platformPrevPosition;
        PlayerLedgeClimbState ledgeClimbState;
        private void Start()
        {
            ledgeClimbState = transform.root.GetComponent<Player>().LedgeClimbState;
        }
        void FixedUpdate()
        {
            if (platform != null&& (Vector2)platform.position != platformPrevPosition)
            {
                Vector2 platformMov = (Vector2)platform.position - platformPrevPosition;
                transform.root.position += (Vector3)platformMov;
                ledgeClimbState.startPos += platformMov;
                ledgeClimbState.stopPos += platformMov;
                platformPrevPosition = platform.position;
                
            }

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("MovePlatform"))
            {
                platform = collision.transform;
                platformPrevPosition = platform.position;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("MovePlatform"))
            {
                platform = null;
            }
        }
    }
}
