using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Avocado
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] GameObject enemyInScene;
        public GameObject enemyPrefab;   
        public EnemyWavesManager wavesManager;
        public Transform spawnPoint;
        GameObject currentEnemy;
        [SerializeField, Range(-1,1)] int spawnDirection=1;
        [SerializeField] List<Vector2> movePositions;
        private void Awake()
        {
            wavesManager = GetComponentInParent<EnemyWavesManager>();
        }
        public void Spawn()
        {
            if (enemyInScene != null) currentEnemy = enemyInScene;
            
            if (currentEnemy != null)
            { 
                currentEnemy.SetActive(true);
                currentEnemy.transform.position = spawnPoint.position;
            }
            else
                currentEnemy=Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            if (currentEnemy.TryGetComponent<FabricOctopusController>(out FabricOctopusController controller) && movePositions.Count>0)
            {
                controller.SetMovePositions(movePositions);
            }
            currentEnemy.transform.localScale = new Vector3(spawnDirection, 1, 1);
            StartCoroutine(EnemyDisableDetector());
        }
        IEnumerator EnemyDisableDetector()
        { 
            yield return new WaitUntil(() => !currentEnemy.activeSelf);
            gameObject.SetActive(false);
            wavesManager.EnemyDie();
        }
        private void OnDrawGizmos()
        {
            if (movePositions.Count!=0 && movePositions!=null)
            {
                for (int i = 0; i < movePositions.Count-1; i++)
                {
                    Gizmos.DrawLine((Vector2)transform.position + movePositions[i], (Vector2)transform.position + movePositions[i + 1]);
                }
            }
        }
    }
}
