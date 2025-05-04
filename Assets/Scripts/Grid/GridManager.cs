using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Avocado
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] GameObject movingPlatformPrefab;
        Queue<GameObject> platformsPool;
        [field: SerializeField]public bool stopInPoints { private set; get; }
        [ field:SerializeField,Range(0.01f, 10)] public float platformSpeed { private set; get; }
        [ field:SerializeField,Range(0.01f, 10)] public float platformStopTime { private set; get; }
        [ field:SerializeField,Range(0.01f, 10)] public float platformMovementTime { private set; get; }

        public static GridManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
           platformsPool= new Queue<GameObject>();
            FillPool();
        }
      
        void FillPool()
        {
            for (int i = 0; i < 10; i++)
            { 
               ExpandPool();
            }
        }
        public void ReturnToPool(GameObject platform)
        { 
            platform.SetActive(false);
            platformsPool.Enqueue(platform);
        }
        public GameObject GetPlatform(Vector2 position, Quaternion rotation)
        {
            if (platformsPool.Count==0)
            {
                ExpandPool ();
            }

             GameObject platform= platformsPool.Dequeue();   
            platform.transform.position = position;
            platform.transform.rotation = rotation; 
            
            platform.SetActive(true);
            return platform;
          
        }
        void ExpandPool()
        { 
            GameObject platform= Instantiate (movingPlatformPrefab);
            platform.SetActive(false);
            platformsPool.Enqueue(platform);  
        }
    }
}
