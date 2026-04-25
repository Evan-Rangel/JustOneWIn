using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Avocado
{
    [Serializable]
    public class EnemyWave
    {
        public EnemySpawn[] spawns;
    }
   
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] string eventName;
        int currentWave;
        int enemiesAlive;
        [SerializeField]EnemyWave[] waves;
        public UnityEvent waveOvercome;
        FabricEnemyCollision fabricEnemyCollision;
        private void Awake()
        {
            fabricEnemyCollision = GetComponentInChildren<FabricEnemyCollision>();
        }
        private void Start()
        {
            if (SaveManager.IsEventKeySaved(eventName))
                gameObject.SetActive(false);
            currentWave = 0;
            enemiesAlive = 0;
        }
        private void OnEnable()
        {
         fabricEnemyCollision.OnPlayerEnter += PlayerCollisionActive;
        }
        private void OnDisable()
        {
            fabricEnemyCollision.OnPlayerEnter -= PlayerCollisionActive;
        }
        public void PlayerCollisionActive(Collider2D _player)
        {
           fabricEnemyCollision.gameObject.SetActive(false);
            StartWave();
        }
        public void StartWave()
        {
            EnemyWave currentEnemyWave = waves[currentWave];
            enemiesAlive = currentEnemyWave.spawns.Length;
            for (int i = 0; i < currentEnemyWave.spawns.Length; i++)
            {
                currentEnemyWave.spawns[i].gameObject.SetActive(true);
            }
        }

        public void EnemyDie()
        { 
            enemiesAlive--;
            if (enemiesAlive <= 0)
            {
                currentWave++;
                if (currentWave < waves.Length)
                    StartWave();
                else
                    waveOvercome.Invoke();
            }
        }

    }
}
