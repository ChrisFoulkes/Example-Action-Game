using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class EnemySpawner : MonoBehaviour
{
    public float count = 0;
    public GameObject enemyPrefab;
    public float spawnDelay = 1f;
    public float spawnRadius = 5f;
    public float minSpawnDistance = 2f;

    private float timeSinceLastSpawn;
    private Transform[] enemyPositions;
    private bool spawnActive = true; 

    void Start()
    {


        PlayerDeathEvent.RegisterListener(OnPlayerDeath);
    }

    void OnDestroy()
    {
        PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
    }

    void FixedUpdate()
    {
        if (spawnActive)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnDelay)
            {
                SpawnEnemy();
            }

        }
    }

    private void SpawnEnemy()
    {
        enemyPositions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemyPositions[i] = transform.GetChild(i);
        }

        timeSinceLastSpawn = 0f;

        Vector2 spawnPosition = GetRandomPosition();
        count = 0;
        while (IsTooCloseToOtherEnemy(spawnPosition))
        {
            spawnPosition = GetRandomPosition();
            count++;
            if (count > 100)  // no valid spawn location found
                Debug.LogWarning("NO VALID SPAWN RADIUS TOO HIGH MOD DENSITY IN SPAWN!!!!");
                break;
        }
        var enemyobj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, gameObject.transform);

        enemyobj.name = "enemy-" + count; 
        count++;
    }

    Vector2 GetRandomPosition()
    {
        Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
        spawnPosition += (Vector2)transform.position;
        return spawnPosition;
    }

    bool IsTooCloseToOtherEnemy(Vector2 position)
    {
        foreach (Transform enemyPos in enemyPositions)
        {
            if (Vector2.Distance(enemyPos.position, position) < minSpawnDistance)
            {
                return true;
            }
        }
        return false;
    }
    void OnPlayerDeath(PlayerDeathEvent death)
    {
        spawnActive = false;
    }
}