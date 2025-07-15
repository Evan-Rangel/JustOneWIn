using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class ZoneManager : NetworkBehaviour
    {
        [SerializeField] ZoneEntity[] spawnPoints;
        [SerializeField] ZoneManager[] linkedZones;
        [SerializeField] GameObject teleport;

        public void Start()
        {
            if (isServer)
            {
                LoadZoneEntities();
            }
        }   
        [Server]
        void LoadZoneEntities()
        {
            foreach (ZoneEntity zoneEntity in spawnPoints)
            {
                GameObject spawnedEntity = Instantiate(zoneEntity.entity, zoneEntity.spawnPoint.position, Quaternion.identity);
                NetworkServer.Spawn(spawnedEntity);
            }
        }
    }
    [Serializable]
    public class ZoneEntity
    { 
        public GameObject entity;
        public Transform spawnPoint;
    }
}

