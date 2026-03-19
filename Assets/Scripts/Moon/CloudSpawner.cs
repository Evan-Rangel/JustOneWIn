using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs;

    public float spawnX = 200f;
    public float minY = -5f;
    public float maxY = 2f;

    public float spawnInterval = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCloud), 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        if (cloudPrefabs.Length == 0) return;

        int index = Random.Range(0, cloudPrefabs.Length);
        GameObject selectedCloud = cloudPrefabs[index];

        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

        Instantiate(selectedCloud, spawnPos, Quaternion.identity);
    }
}