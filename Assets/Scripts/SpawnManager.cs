using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] playerPrefabs;

    public int waveNumber = 1;
    public int playerCount = 4;

    private float spawnRange = 9.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerPrefab = playerPrefabs[i];
            // playerPrefab.SetActive(true);
            Instantiate(playerPrefab, GetSpawnPos(), playerPrefab.transform.rotation);
        }
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, GetSpawnPos(), enemyPrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int totalEnemies = FindObjectsOfType<Enemy>().Length;
        if (totalEnemies == 0)
        {
            waveNumber++;
            SpawnEnemies();
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