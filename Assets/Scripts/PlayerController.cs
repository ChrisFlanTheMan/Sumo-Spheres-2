using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    public float speed = 750.0f;
    public float powerupStrength = 15.0f;
    public bool hasPowerup = false;

    public GameObject powerupIndicator;
    public GameObject projectilePrefab;

    public InputAction playerJoystick;
    public InputAction playerJump;

    private int powerupNumber;
    private bool canLaunchProjectile = true;

    private Rigidbody playerRb;

    private void OnEnable()
    {
        playerJoystick.Enable();
        playerJump.Enable();
    }

    private void OnDisable()
    {
        playerJoystick.Disable();
        playerJump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controlDirection = playerJoystick.ReadValue<Vector3>();
        bool jumpPressed = playerJump.IsPressed();

        int groundLayer = LayerMask.NameToLayer("Ground");

        bool grounded = Physics.Raycast(playerRb.transform.position, Vector3.down, 1f, groundLayer);

        float frictionSpeed = speed / 12f;

        if (jumpPressed && grounded) {
            playerRb.AddForce(new Vector3(0f, 10f, 0f) * speed * Time.deltaTime);
        }

        if (grounded) {
            playerRb.AddForce(controlDirection * speed * Time.deltaTime);
            playerRb.AddForce(playerRb.velocity * -frictionSpeed * Time.deltaTime);
        } else {
            float controlToVelocityAlignment = Vector3.Dot(controlDirection, playerRb.velocity);
            playerRb.AddForce(controlDirection * Math.Max(-controlToVelocityAlignment, 0) * frictionSpeed * Time.deltaTime);
        }

        powerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);

        if (hasPowerup && powerupNumber == 1 && canLaunchProjectile)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                launchProjectileAtEnemy(enemies[i]);
            }

            canLaunchProjectile = false;
            StartCoroutine(ProjectileCountdown());
        }
    }

    private void launchProjectileAtEnemy(GameObject enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(1, 0, 0), projectilePrefab.transform.rotation);
        projectile.GetComponent<Projectile>().SetEnemyTarget(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);

            powerupNumber = 1;//Random.Range(0, 2);

            Destroy(other.gameObject);

            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    IEnumerator ProjectileCountdown()
    {
        yield return new WaitForSeconds(1);
        canLaunchProjectile = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup && powerupNumber == 0)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}
