using Mirror;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Avocado
{
    public class LevelController : NetworkBehaviour
    {

        List<ZoneManager> zones= new List<ZoneManager>();
        List<ZoneManager> activeZones= new List<ZoneManager>();
        int maxActiveZones = 9;
        [SerializeField]Tilemap tilemap;

        ZoneManager PrevZoneNeighbour(int index)
        {
            if (index <=0)
            {
                Debug.LogWarning("No Zones");
                return null;
            }
            ZoneManager zone;

            zone = activeZones[index-1];
            if (zone.neighborZones.Count > 0)
            {
                return zone.neighborZones[UnityEngine.Random.Range(0, zone.neighborZones.Count)];
            }
            else
            {
                zone= PrevZoneNeighbour(index - 1);
                return zone;
            }

        }
        private void Start()
        {
            Debug.Log("a");
            Debug.Log(tilemap.size);
            /*
            ZoneManager zone = zones[UnityEngine.Random.Range(0, zones.Count)];
            for (int i = 0; i < maxActiveZones; i++)
            {
                activeZones.Add(zone);
                zones.Remove(zone);
                if (zone.neighborZones.Count>0)
                {
                    zone= zone.neighborZones[UnityEngine.Random.Range(0, zone.neighborZones.Count)];
                    zone.neighborZones.Remove(zone);
                }
                else
                {
                    zone = PrevZoneNeighbour(activeZones.Count);
                }
              
            }

            foreach (ZoneManager activeZone in activeZones)
            {
                if (activeZone.isServer)
                {
                    activeZone.LoadZoneEntities();
                }
            }
            */
        }

        void RandomizeZones()
        { 
            
        }
    }
}
