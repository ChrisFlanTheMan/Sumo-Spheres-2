using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public float speed = 500.0f;

    private Rigidbody enemyRb;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.AddForce(GetLookDirection() * speed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

    }

    private Vector3 GetLookDirection()
    {
        return (player.transform.position - transform.position).normalized;
    }
}
