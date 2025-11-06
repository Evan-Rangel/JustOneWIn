using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Avocado
{
    public class PlatformPointsManager : MonoBehaviour
    {
        [SerializeField] bool activatePlat = false;

        bool inMovement = false;

        [Header("Prefabs")]
        [SerializeField] GameObject pointPrefab;
        [SerializeField] GameObject movingPlatformPrefab;
        [Header("Platform Movement Timers")]
        [SerializeField] float stopTime;
        [Header("Points and Platforms")]
        [SerializeField, Range(1, 25)] float platformSpeed = 5f;    
        [SerializeField, Range(0.1f, 10)] float platformAcceleration = 2f;    
         List<GameObject> platformsPool;
         List<PlatformMovement> activePlatforms;
        [field:SerializeField, Range(1, 100)] public int numPoints = 1;
        public List<GameObject> points =new List<GameObject>();

       

        IEnumerator spawnPlatform, platformsStop;
        private void Awake()
        {
            activatePlat = false;
            activePlatforms = new List<PlatformMovement>();
            platformsPool = new List<GameObject>();
            spawnPlatform = SpawnPlatform();
            platformsStop = PlatformsStop();
            FillPool();
        }
        public void DisablePlatforms()
        {
        }
        public void ActivatePlatforms()
        {
            spawnPlatform = SpawnPlatform();
            StartCoroutine(spawnPlatform);
        }
        private void Update()
        {
            if (activatePlat)
            {
                ActivatePlatforms();
                activatePlat = false;
            }

            PlatformsMovement();
        }
        void PlatformsMovement()
        {
            if (!inMovement) return;
            for (int i = 0; i < activePlatforms.Count; i++)
            {
                
                if (activePlatforms[i].DistanceToPoint()<0.01f)
                {
                    activePlatforms[i].transform.position = activePlatforms[i].targetPoint.position;

                    if (activePlatforms[i].targetPoint.GetComponent<PlatformPoint>().spawnPlatformOnPoint)
                    {
                        spawnPlatform = SpawnPlatform();
                        StartCoroutine(spawnPlatform);
                    }

                    int nextIndex = points.FindIndex(x => x.Equals(activePlatforms[i].targetPoint.gameObject)) + 1;
                    if (nextIndex >= points.Count)
                    {
                        ReturnToPool(activePlatforms[i].gameObject);
                        continue;
                    }

                    Transform nextTarget = points[nextIndex].transform;
                    activePlatforms[i].targetPoint = nextTarget;
                }
            }
        }
        IEnumerator SpawnPlatform()
        {
            
            platformsStop = PlatformsStop();
            StartCoroutine(platformsStop);

            yield return new WaitUntil(()=>inMovement);
                
            PlatformMovement platform = GetPlatform(points[0].transform.position, Quaternion.identity);
            platform.InitMovement(points[1].transform, platformSpeed,platformAcceleration);
                
            activePlatforms.Add(platform);
            //yield return Helpers.GetWait(spawnPlatformTime);
        }
        IEnumerator PlatformsStop()
        {
            inMovement = false;
            yield return Helpers.GetWait(stopTime);
            inMovement = true;
        }
        #region Pool Methods
        void FillPool()
        {
            for (int i = 0; i < 3; i++)
            {
                ExpandPool();
            }
        }
        public void ReturnToPool(GameObject platform)
        {
            platformsPool.Add(platform);
            activePlatforms.Remove(platform.GetComponent<PlatformMovement>());
            platform.SetActive(false);
        }
        public PlatformMovement GetPlatform(Vector2 position, Quaternion rotation)
        {
            if (platformsPool.Count == 0)
            {
                ExpandPool();
            }

            GameObject platform = platformsPool[0];
            platformsPool.RemoveAt(0);
            platform.transform.position = position;
            platform.transform.rotation = rotation;

            platform.SetActive(true);
            return platform.GetComponent<PlatformMovement>();

        }
        void ExpandPool()
        {
            GameObject platform = Instantiate(movingPlatformPrefab);
            platform.SetActive(false);
            platformsPool.Add(platform);
        }
        #endregion
        #region Point Methods
        public void ClearPoints()
        {
            foreach (GameObject child in points)
            {
                DestroyImmediate(child.gameObject);
            }
            points.Clear();
        }
        public void GeneratePoints()
        {
            ClearPoints();

            for (int i = 0; i < numPoints; i++)
            {
                GameObject newPoint = InstantiatePoint();
                newPoint.name = "Point" + (i + 1);
                points.Add(newPoint);
                newPoint.transform.position = newPoint.transform.position + new Vector3(i * 2, 0, 0);
                if (i>0)
                {
                    points[i - 1].GetComponent<PlatformPoint>().nextPoint = newPoint.transform ;
                }
            }
        }
        private GameObject InstantiatePoint()
        {
            if (pointPrefab == null) return null;
            return (GameObject)PrefabUtility.InstantiatePrefab(pointPrefab, transform);
        }
        #endregion
    }
}
