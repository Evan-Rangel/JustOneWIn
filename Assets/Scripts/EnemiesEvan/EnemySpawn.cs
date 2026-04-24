using System;
using System.Collections;
using UnityEngine;

namespace Avocado
{
    public class EnemySpawn : MonoBehaviour
    {
        public GameObject enemyPrefab;   
        public EnemyWavesManager wavesManager;
        public Transform spawnPoint;
        GameObject currentEnemy;

        private void Awake()
        {
            wavesManager = GetComponentInParent<EnemyWavesManager>();
        }
        public void Spawn()
        {
            currentEnemy=Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            StartCoroutine(EnemyDisableDetector());
        }

        IEnumerator EnemyDisableDetector()
        { 
            yield return new WaitUntil(() => !currentEnemy.activeSelf);
            wavesManager.EnemyDie();
        }
    }
}
