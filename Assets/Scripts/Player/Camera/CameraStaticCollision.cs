using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class CameraStaticCollision : MonoBehaviour
    {
        [SerializeField] Transform staticPoint;
        [SerializeField] int direction;
        [SerializeField] bool staticX, staticY;
        Transform playerTransform;
        Transform cameraHolder;
        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void Update()
        {
            if (playerTransform == null || cameraHolder==null) return;

            if (staticX)
            {
                if (direction > 0 && playerTransform.position.x >= staticPoint.position.x)
                { 
                    cameraHolder.position= new Vector3(staticPoint.position.x, playerTransform.position.y, 0);
                    return;
                }
                if (direction < 0 && playerTransform.position.x <= staticPoint.position.x)
                {
                    cameraHolder.position = new Vector3(staticPoint.position.x, playerTransform.position.y, 0);
                    return;
                }
                else
                { 
                    cameraHolder.position = new Vector3(playerTransform.position.x, playerTransform.position.y, 0);
                    return;
                }
            
            }
            if (staticY)
            {
                if (direction > 0 && playerTransform.position.y >= staticPoint.position.y)
                {
                    cameraHolder.position = new Vector3(playerTransform.position.x, staticPoint.position.y, 0);
                    return;
                }
                if (direction < 0 && playerTransform.position.y <= staticPoint.position.y)
                {
                    cameraHolder.position = new Vector3(playerTransform.position.y, staticPoint.position.y, 0);
                    return;
                }
                else
                {
                    cameraHolder.position = new Vector3(playerTransform.position.x, playerTransform.position.y, 0);
                    return;
                }
            }
        }
        public void PlayerEnterCollision(Transform _cameraHolder)
        {
            cameraHolder = _cameraHolder;
            cameraHolder.parent = transform;
        }

        public void PlayerExitCollision()
        { 
            cameraHolder = null;
        }
    }
}
