using System;
using UnityEngine;

namespace Avocado
{
    [Serializable]
    public class TpEntity
    {
        public Transform pos;
        public string zoneName;
        public string sceneName;
    }
    public class MinimapCamera : MonoBehaviour
    {
        Camera cam;
        public static MinimapCamera Instance;
        Vector2 currentDirection;
       // [SerializeField] MinimapUIManager minimapUIManager;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            cam=GetComponentInChildren<Camera>();
        }
        private void Update()
        {
            transform.position+= (Vector3)currentDirection; 
        }
        public void SetDirection(Vector2 dir)
        {
            currentDirection = dir;
        }
        public void ZoomIn()
        {
            if (cam.fieldOfView > 165)
                cam.fieldOfView -= 1;
        }
        public void ZoomOut()
        {
            if (cam.fieldOfView<175)
                cam.fieldOfView += 1;
        }
    }
}
