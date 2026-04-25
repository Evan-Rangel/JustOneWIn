using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricSearcherSpawner : MonoBehaviour
    {
        [SerializeField, Range(-1,1), Tooltip("-1 for left +1 for right")] int direction;
        Animator animator;
        [SerializeField] Transform spawnPoint;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void CancelRepeating()
        {

            CancelInvoke(nameof(ActiveSpawn));
        }
        private void Start()
        {
            InvokeRepeating(nameof(ActiveSpawn), 1f, 3f);
        }
        void ActiveSpawn()
        { 
            animator.SetTrigger("Open");
        }

        void SpawnFabricSearcherPrefab()
        {
            GameObject searcher = FabricEnemiesPool.Instance.GetFabricSearcher();
            searcher.transform.position = spawnPoint.position;
            searcher.GetComponent<FabricSearcherController>().SetValue(-direction);
            
        }
    }
}
