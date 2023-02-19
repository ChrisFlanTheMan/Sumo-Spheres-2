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

    public InputAction playerControls;

    private int powerupNumber;
    private bool canLaunchProjectile = true;

    private Rigidbody playerRb;

    private Vector3 moveDirection = Vector3.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector3>();
        playerRb.AddForce(moveDirection * speed * Time.deltaTime);

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
