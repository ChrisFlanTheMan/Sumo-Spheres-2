using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static event Action<int> OnPlayerStart;

    public GameObject[] enemyPrefabs;
    public GameObject[] playerPrefabs;

    public int waveNumber = 1;
    public int playerCount = 5;

    private float spawnRange = 9.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        OnPlayerStart?.Invoke(playerCount);
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

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 GetSpawnPos()
    {
        return new Vector3(GetRandomCoordinatePos(), 0, GetRandomCoordinatePos());
    }

    private float GetRandomCoordinatePos()
    {
        return UnityEngine.Random.Range(-spawnRange, spawnRange);
    }
}