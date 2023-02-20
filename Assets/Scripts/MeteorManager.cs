using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    public GameObject meteorPrefab;

    private float spawnRangeMin = 20.0f;
    private float spawnRangeMax = 40.0f;


    private float startDelay = 2.0f;
    private float spawnInterval = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        startDelay = Random.Range(2.5f, 4.0f);
        spawnInterval = Random.Range(1.0f, 8.0f);
        InvokeRepeating("SpawnMeteor", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnMeteor()
    {
        GameObject powerup = Instantiate(meteorPrefab, RandomSpawnPosition(), meteorPrefab.transform.rotation);
        Destroy(powerup, 10.0f);
    }

    private Vector3 RandomSpawnPosition()
    {
        float spawnX = getDirection() * Random.Range(spawnRangeMin, spawnRangeMax);
        float spawnZ = getDirection() * Random.Range(spawnRangeMin, spawnRangeMax);

        return new Vector3(spawnX, 0, spawnZ);
    }

    private int getDirection()
    {
        return Random.Range(0, 2) * 2 - 1;
    }
}
