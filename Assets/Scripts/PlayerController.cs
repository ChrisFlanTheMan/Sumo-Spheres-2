using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{ 
    public float speed = 1000.0f;
    public float powerupStrength = 15.0f;
    public bool hasPowerup = false;

    public bool readyToJump = false;

    public GameObject powerupIndicator;
    public GameObject projectilePrefab;

    public AudioClip crashSound;
    public AudioClip jumpSound;

    public InputAction playerJoystick;
    public InputAction playerJump;
    public TextMeshProUGUI deathText;
    public ParticleSystem explode;

    private int powerupNumber;
    private bool canLaunchProjectile = true;
    private int deathCounter;
    private Vector3 startPosition;

    private Rigidbody playerRb;
    private AudioSource playerAudio;

    private void OnEnable()
    {
        playerJoystick.Enable();
        playerJump.Enable();
        startPosition = transform.position;
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
        playerAudio = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controlDirection = playerJoystick.ReadValue<Vector3>();
        bool jumpPressed = playerJump.IsPressed();

        int groundLayer = LayerMask.NameToLayer("Ground");

        bool playerNotMovingVertically = Math.Abs(playerRb.velocity.y) < 0.001f;
        bool groundBeneathPlayer = Physics.Raycast(playerRb.transform.position, Vector3.down, 0.8f, groundLayer);
        bool grounded = playerNotMovingVertically && groundBeneathPlayer;

        float frictionSpeed = speed / 20f;
        float airControlSpeed = speed / 14f;

        Debug.Log(transform.position.ToString());
        // Respawn
        if (transform.position.y < -10)
        {
            Respawn();
        }

        if (grounded) {
            if (!readyToJump) {
                StartCoroutine(ReadyToJumpCountdown());
            }

            if (readyToJump && jumpPressed) {
                playerRb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                playerAudio.PlayOneShot(jumpSound, 1.0f);

                readyToJump = false;
            }

            playerRb.AddForce(controlDirection * speed * Time.deltaTime);
            playerRb.AddForce(playerRb.velocity * -frictionSpeed * Time.deltaTime);
        } else {
            float controlToVelocityAlignment = Vector3.Dot(controlDirection, playerRb.velocity);
            playerRb.AddForce(controlDirection * Math.Max(-controlToVelocityAlignment, 0) * airControlSpeed * Time.deltaTime);
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

    IEnumerator ExplodeCountdown()
    {
        yield return new WaitForSeconds(9);
//        DestroyImmediate(explode, true);
    }

    IEnumerator ReadyToJumpCountdown()
    {
        yield return new WaitForSeconds(0.3f);
        readyToJump = true;
    }

    IEnumerator ImpactPause(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;
            float collisionAlignedEnemySpeed = Vector3.Dot(awayFromPlayer, enemyRb.velocity);
            float collisionAlignedPlayerSpeed = Vector3.Dot(-awayFromPlayer, playerRb.velocity);

            // Unity collision already happened, just amplify the result
            playerRb.AddForce(-awayFromPlayer * collisionAlignedPlayerSpeed * 0.8f * getPowerupForceModifier(), ForceMode.Impulse);

            float totalCollisionAlignedSpeed = collisionAlignedEnemySpeed + collisionAlignedPlayerSpeed;
            float collisionScaling = Mathf.Pow(10f, Math.Min(1f, totalCollisionAlignedSpeed/20f))/10f;
            playerAudio.PlayOneShot(crashSound, collisionScaling);

            float pauseAmount = Mathf.Lerp(0f, 0.2f, collisionScaling);
            StartCoroutine(ImpactPause(pauseAmount));
        }
        if (collision.gameObject.CompareTag("Sun"))
        {
            Instantiate(explode, transform.position, Quaternion.identity);
            explode.Play();
            StartCoroutine(ExplodeCountdown());
            Respawn();
        }
    }

    private void Respawn()
    {
        deathCounter++;
        //deathText.SetText("Player: " + deathCounter);
        playerRb.velocity = Vector3.zero;
        playerRb.rotation = Quaternion.identity;
        playerRb.angularVelocity = Vector3.zero;
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        transform.position = startPosition + new Vector3(0, 5, 0);
    }

    private float getPowerupForceModifier()
    {
        return hasPowerup ? powerupStrength : 1.0f;
    }
}
