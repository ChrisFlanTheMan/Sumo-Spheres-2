using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject powerupPrefab;

    public int waveNumber = 1;

    private float spawnRange = 9.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
        SpawnPowerup();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, GetSpawnPos(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerup()
    {
        Instantiate(powerupPrefab, GetSpawnPos(), powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        int totalEnemies = FindObjectsOfType<Enemy>().Length;
        if (totalEnemies == 0)
        {
            waveNumber++;
            SpawnEnemies();
            SpawnPowerup();
        }
    }

    private Vector3 GetSpawnPos()
    {
        return new Vector3(GetRandomCoordinatePos(), 0, GetRandomCoordinatePos());
    }

    private float GetRandomCoordinatePos()
    {
        return Random.Range(-spawnRange, spawnRange);
    }
}
