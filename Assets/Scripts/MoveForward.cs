using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;
    public float turnSpeed = 10.0f;

    public GameObject sun;

    private Vector3 moveDirection;
    private float spawnRangeMin = 0f;
    private float spawnRangeMax = 10f;
    private Vector3 randomOffset;

    private float xRange = 50.0f;

    void Start()
    {
        GameObject sun = GameObject.Find("Sun");
	float randomXOffset = Random.Range(spawnRangeMin, spawnRangeMax);
	float randomYOffset = Random.Range(spawnRangeMin, spawnRangeMax);
        randomOffset = new Vector3(randomXOffset, 0, randomYOffset);
        moveDirection = (sun.transform.position + randomOffset - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime * speed, Space.World);
        transform.Rotate(new Vector3(0, turnSpeed, 0) * Time.deltaTime);

        if (transform.position.x < -xRange || transform.position.x > xRange)
        {
            Destroy(gameObject);
        }
    }
}
