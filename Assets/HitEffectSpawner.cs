using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HitEffectSpawner : MonoBehaviour
{
    public static HitEffectSpawner Instance { get; private set; }

    [SerializeField] private GameObject hitEffectPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void SpawnHitEffect(Vector2 collisionPoint, Transform enemyTransform)
    {
        // Calculate the direction vector from the enemy's center to the collision point
        Vector2 direction = (collisionPoint - (Vector2)enemyTransform.position).normalized;

        // Calculate the angle in degrees for the hit effect's rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Ensure the hit effect spawns on top of the enemy sprite
        Vector3 spawnPositionWithZ = new Vector3(collisionPoint.x, collisionPoint.y, enemyTransform.position.z - 0.1f);

        // Instantiate the hit effect at the chosen position and rotation
        GameObject hitEffect = Instantiate(hitEffectPrefab, spawnPositionWithZ, Quaternion.Euler(0, 0, angle));

        // Set the hit effect as a child of the enemy transform, so it moves with the enemy
        hitEffect.transform.SetParent(enemyTransform);
    }
}