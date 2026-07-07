using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Avocado
{
    public class FabricEnemiesPool : MonoBehaviour
    {
        [SerializeField] GameObject searcherPrefab;
        List<GameObject> searcherPool = new List<GameObject>();
        [SerializeField] GameObject fabricOctopusBulletPrefab;
        List<GameObject> fabricOctopusBulletPool = new List<GameObject>();
        [SerializeField] GameObject fabricMiniBossBulletPrefab;
        List<GameObject> fabricMiniBossBulletPool = new List<GameObject>();
        [SerializeField] GameObject fabricTorretBulletPrefab;
        List<GameObject> fabricTorretBulletPool = new List<GameObject>();


        [SerializeField] GameObject explosionEffectPefab;
        List<GameObject> explosionEffectPool = new List<GameObject>();



        public static FabricEnemiesPool Instance;
        private void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            fabricMiniBossBulletPool.Clear();
            fabricOctopusBulletPool.Clear(); 
            fabricTorretBulletPool.Clear();
            searcherPool.Clear();
            explosionEffectPool.Clear();

        }
        public GameObject GetFabricTorretBullet()
        {
            foreach (GameObject bullet in fabricTorretBulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    return bullet;
                }
            }
            GameObject newBullet = Instantiate(fabricTorretBulletPrefab);
            fabricTorretBulletPool.Add(newBullet);
            return newBullet;
        }

        public GameObject GetMiniBossBullet()
        {

            foreach (GameObject bullet in fabricMiniBossBulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    return bullet;
                }
            }
            GameObject newBullet = Instantiate(fabricMiniBossBulletPrefab);
            fabricMiniBossBulletPool.Add(newBullet);
            return newBullet;

        }
        public GameObject GetOctopusBullet()
        {
            foreach (GameObject bullet in fabricOctopusBulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    return bullet;
                }
            }
            GameObject newBullet = Instantiate(fabricOctopusBulletPrefab);
            fabricOctopusBulletPool.Add(newBullet);
            return newBullet;
        }
        public GameObject GetFabricSearcher()
        {
            foreach (GameObject searcher in searcherPool)
            {
                if (!searcher.activeInHierarchy)
                {
                    searcher.SetActive(true);
                    return searcher;
                }
            }
            GameObject newSearcher = Instantiate(searcherPrefab);
            searcherPool.Add(newSearcher);
            return newSearcher;
        }
        public GameObject GetExplosionEffect()

        {
            foreach (GameObject effect in explosionEffectPool)
            {
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    return effect;
                }
            }
            GameObject newEffect = Instantiate(explosionEffectPefab);
            explosionEffectPool.Add(newEffect);
            return newEffect;
        }
    }
}
