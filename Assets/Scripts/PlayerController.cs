using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] float laserSpeed = 15f;
    [SerializeField] GameObject playerLaserPrefab;
    [SerializeField] float firingRate = 0.2f;
    float firingHeight = 1.1f;

    float health = 1000f;
    float minX, maxX;
    float width;

    AudioSource playerAudioSource;
    [SerializeField] AudioClip shootingClip;

    HPDisplay hPDisplay;

    void Start()
    {
        hPDisplay = transform.GetChild(0).gameObject
            .transform.GetChild(0).gameObject
            .GetComponent<HPDisplay>();
        hPDisplay.SetMaxHP(health);

        playerAudioSource = GetComponent<AudioSource>();
        width = GetComponent<PolygonCollider2D>().bounds.size.x;

        // Calculate the range in which Spaceship can be
        float distanceToCamera = Camera.main.transform.position.z - transform.position.z;
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera)).x + width / 2;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera)).x - width / 2;
    }

    // Firing
    void Fire()
    {
        Vector3 firingOffset = new Vector3(0, firingHeight);
        Vector3 firingPosition = transform.position + firingOffset;

        GameObject laser = Instantiate(playerLaserPrefab, firingPosition, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = Vector3.up * laserSpeed;

        playerAudioSource.PlayOneShot(shootingClip);
    }

    void Update()
    {
        // Firing input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.0000001f, firingRate);
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        // Spaceship movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // Put Spaceship back to movable area
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y);
    }

    private void UpdateExistingState()
    {
        if (health < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        // Enemy is hit by a laser
        if (projectile)
        {
            health -= projectile.GetDamage();
            hPDisplay.UpdateHPBar(health);
            UpdateExistingState();

            // Remove laser from screen
            projectile.Hit();
        }
    }
}
