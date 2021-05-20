using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    float spawnDelay = 0.5f;

    float width = 8f;
    float height = 4f;

    float minX = float.MaxValue, minY = float.MaxValue;
    float maxX = float.MinValue, maxY = float.MinValue;

    bool isMovingRight = true;
    float movingSpeed = 5f;

    float leftBoundary;
    float rightBoundary;

    // Update formation size based on objects inside
    private void UpdateFormationSize(GameObject obj)
    {
        PolygonCollider2D objectCollider = obj.GetComponent<PolygonCollider2D>();
        Vector3 topLeft = objectCollider.bounds.min;
        Vector3 bottomRight = objectCollider.bounds.max;

        minX = topLeft.x < minX ? topLeft.x : minX;
        maxX = bottomRight.x > maxX ? bottomRight.x : maxX;

        minY = topLeft.y < minY ? topLeft.y : minY;
        maxY = bottomRight.y > maxY ? bottomRight.y : maxY;

        width = maxX - minX;
        height = maxY - minY;
    }

    // Draw gizmo using for Editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y),
            new Vector3(width, height));
    }

    // Start is called before the first frame update
    void Start()
    {
        float distanceToCamera = Camera.main.transform.position.z - transform.position.z;
        leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera)).x + width / 2;
        rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera)).x - width / 2;

        SpawnUntilFull();
    }
    
    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            // Create new Enemy object
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, enemyPrefab.transform.rotation);
            enemy.transform.parent = child;

            UpdateFormationSize(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update formation position
        if (isMovingRight)
        {
            transform.position += Vector3.right * movingSpeed * Time.deltaTime;
        } else
        {
            transform.position += Vector3.left * movingSpeed * Time.deltaTime;
        }

        // Change moving direction in needed
        if (transform.position.x > rightBoundary) isMovingRight = false;
        else if (transform.position.x < leftBoundary) isMovingRight = true;

        if (AllEnemiesIsDead()) SpawnUntilFull();
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();

        if (freePosition)
        {
            // Create new Enemy object
            GameObject enemy = Instantiate(enemyPrefab, freePosition.transform.position, enemyPrefab.transform.rotation);
            enemy.transform.parent = freePosition;
        }

        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0) return child;
        }

        return null;
    }

    bool AllEnemiesIsDead()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0) return false;
        }

        return true;
    }
 }
