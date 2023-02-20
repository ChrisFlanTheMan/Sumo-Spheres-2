using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMeteor : MonoBehaviour
{
    private Rigidbody meteorRb;

    public float speed = 40.0f;
    public float turnSpeed = 10.0f;

    public GameObject sun;
    public GameObject explosionPrefab;

    private Vector3 moveDirection;
    private float spawnRangeMin = 0f;
    private float spawnRangeMax = 10f;
    private Vector3 randomOffset;

    private float xRange = 50.0f;

    void Start()
    {
        GameObject sun = GameObject.Find("Sun");
        meteorRb = GetComponent<Rigidbody>();
        float randomXOffset = Random.Range(spawnRangeMin, spawnRangeMax);
        float randomYOffset = Random.Range(spawnRangeMin, spawnRangeMax);
        randomOffset = new Vector3(randomXOffset, 0, randomYOffset);
        moveDirection = (sun.transform.position + randomOffset - transform.position).normalized;
        meteorRb.velocity = (moveDirection* speed);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(new Vector3(0, turnSpeed, 0) * Time.deltaTime);

        if (transform.position.x < -xRange || transform.position.x > xRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sun"))
        {
            GameObject instance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject.Destroy(instance.gameObject, 2.5f);
            Destroy(this.gameObject);
        }
    }
}
