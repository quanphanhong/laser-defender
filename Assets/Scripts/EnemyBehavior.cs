using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float shotsPerSecond = 0.5f;
    float firingHeight = -0.8f;
    bool allowFiring = true;

    [SerializeField] float health = 150f;

    float enemyScore = 125f;
    ScoreKeeper scoreKeeper;

    [SerializeField] AudioClip shootingClip;
    [SerializeField] AudioClip explodingClip;
    AudioSource enemyAudioSource;

    [SerializeField] float dyingDelay = 0.4f;

    HPDisplay hPDisplay;

    private void Start()
    {
        hPDisplay = transform.GetChild(0).gameObject
            .transform.GetChild(0).gameObject
            .GetComponent<HPDisplay>();
        hPDisplay.SetMaxHP(health);

        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
        enemyAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float probability = shotsPerSecond * Time.deltaTime;
        if (Random.value < probability && allowFiring)
        {
            Fire();
        }
    }

    // Firing
    private void Fire()
    {
        Vector3 firingOffset = new Vector3(0, firingHeight);
        Vector3 firingPosition = transform.position + firingOffset;

        GameObject laser = Instantiate(enemyLaserPrefab, firingPosition, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = Vector3.down * laserSpeed;

        enemyAudioSource.PlayOneShot(shootingClip);
    }

    private void UpdateExistingState()
    {
        if (health <= 0)
        {
            allowFiring = false;
            StartCoroutine("PlayerIsDead");
        }
    }

    IEnumerator PlayerIsDead()
    {
        scoreKeeper.UpdateScore(enemyScore);
        enemyAudioSource.PlayOneShot(explodingClip);
        yield return new WaitForSeconds(dyingDelay);
        Destroy(gameObject);
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
