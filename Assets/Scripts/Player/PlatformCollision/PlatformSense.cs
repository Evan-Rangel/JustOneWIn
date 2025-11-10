using UnityEngine;

namespace Avocado
{
    public class PlatformSense : MonoBehaviour
    {
        Transform platform;
        Vector2 platformPrevPosition;
        PlayerLedgeClimbState ledgeClimbState;
        PlayerWallGrabState wallGrabState;
        private void Start()
        {
            ledgeClimbState = transform.root.GetComponent<Player>().LedgeClimbState;
            wallGrabState = transform.root.GetComponent<Player>().WallGrabState;
        }
        void FixedUpdate()
        {
            if (platform != null&& (Vector2)platform.position != platformPrevPosition)
            {
                Vector2 platformMov = (Vector2)platform.position - platformPrevPosition;
                transform.root.position += (Vector3)platformMov;
                ledgeClimbState.startPos += platformMov;
                ledgeClimbState.stopPos += platformMov;
                wallGrabState.holdPosition += platformMov;
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
            else if (platform!=null )
            {
                ledgeClimbState.platformClimbing = true;
                wallGrabState.releaseGrabWall = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            ledgeClimbState.platformClimbing = false;
            wallGrabState.releaseGrabWall = false;

            if (collision.transform.CompareTag("MovePlatform"))
            {
                platform = null;
            }
        }
    }
}
