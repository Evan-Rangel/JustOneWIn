using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Avocado
{
    public class ZoneManager : NetworkBehaviour
    {
        Dictionary<EntityType, Transform> zonePoints;
        ZoneType currentZoneType;


        [SerializeField] List<GameObject> walls;
        [field: SerializeField] public List<ZoneManager> neighborZones { get; private set; }

        [Server]
        public void LoadZoneEntities()
        {
            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                zonePoints[type] = transform.GetChild((int)type);
            }
            switch (currentZoneType)
            {
                case ZoneType.ENEMY:
                    Transform[] points = zonePoints[EntityType.ENEMY1].GetComponentsInChildren<Transform>();
                    foreach (Transform point in points)
                    {
                        GameObject spawnedEnemy = Instantiate(GameManager.instance.enemy1Prefab, point.position, Quaternion.identity);
                        NetworkServer.Spawn(spawnedEnemy);
                    }
                    points = zonePoints[EntityType.ENEMY2].GetComponentsInChildren<Transform>();
                    foreach (Transform point in points)
                    {
                        GameObject spawnedEnemy = Instantiate(GameManager.instance.enemy2Prefab, point.position, Quaternion.identity);
                        NetworkServer.Spawn(spawnedEnemy);
                    }
                    break;
                case ZoneType.PUZZLE:
                    break;
                case ZoneType.REWARD:
                    break;
                case ZoneType.BOSS:
                    break;
                case ZoneType.WALL:
                    foreach (GameObject wall in walls)
                    {
                        wall.SetActive(true);
                        LoadWalls(walls.IndexOf(wall));
                    }
                    break;

            }
        }
        [ClientRpc]
        void LoadWalls(int idx)
        {
            walls[idx].SetActive(true);
        }
    }
  

    public enum ZoneType
    { 
        ENEMY,
        PUZZLE,
        REWARD,
        BOSS,
        WALL

    } 
    public enum EntityType
    { 
        ENEMY1,
        ENEMY2,
        PUZZLE,
        REWARD,
        BOSS,
        WALL,
        PLATFORM_POINT

    }
}

