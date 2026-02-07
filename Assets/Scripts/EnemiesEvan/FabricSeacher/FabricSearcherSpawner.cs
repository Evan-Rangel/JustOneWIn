using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricSearcherSpawner : MonoBehaviour
    {
        [SerializeField, Range(-1,1), Tooltip("-1 for left +1 for right")] int direction;
        private void Start()
        {
            InvokeRepeating("SpawnFabricSearcherPrefab", 1f, 3f);
        }
        void SpawnFabricSearcherPrefab()
        {

            GameObject searcher = FabricEnemiesPool.Instance.GetFabricSearcher();
            searcher.transform.position = transform.position;
            searcher.GetComponent<FabricSearcherController>().SetValue(-direction);
            
        }
    }
}
