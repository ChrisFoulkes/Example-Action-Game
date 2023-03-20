using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class EnemySpawner : MonoBehaviour
{
    public float count = 0;
    public float spawnRadius = 5f;
    public float minSpawnDistance = 2f;

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
    }

    public bool IsSpawnActive() 
    {
        return spawnActive;
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        enemyPositions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemyPositions[i] = transform.GetChild(i);
        }

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

        return enemyobj;
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