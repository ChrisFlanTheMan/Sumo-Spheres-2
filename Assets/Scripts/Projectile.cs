using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 500.0f;
    public float powerupStrength = 15.0f;

    private Rigidbody projectileRb;
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            projectileRb.AddForce(GetLookDirection() * speed * Time.deltaTime);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyTarget(GameObject enemy)
    {
        this.enemy = enemy;
    }

    private Vector3 GetLookDirection()
    {
        return (enemy.transform.position - transform.position).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromProjectile = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromProjectile * powerupStrength, ForceMode.Impulse);
        }
    }
}
