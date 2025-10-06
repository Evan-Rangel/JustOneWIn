using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class EntitySpawn : MonoBehaviour
    {
        [SerializeField]
        protected Transform spawnPoint;
        public Action<Transform, GameObject> spawn;
        public void SpawnEntity()
        {
            
        }
    }
}
