using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossAttackController : EnemyAttackController
{
    [Header("Projectile Settings")]
    public int numberOfProjectiles = 3;
    public float projectileSpacing = 1.0f;
    private List<GameObject> spawnedProjectiles = new List<GameObject>();

    public float yOffset = 0f;
    public float xOffset = 0f;
    protected override IEnumerator StartAttack(float animDuration)
    {
        isInAttack = true;
        StartCoroutine(SpawnHitBox());
        yield return new WaitForSeconds(animDuration + (0.15f*numberOfProjectiles));
        isInAttack = false;  
        
        // Destroy the spawned projectiles
        foreach (GameObject projectile in spawnedProjectiles)
        {
            Destroy(projectile);
        }
        spawnedProjectiles.Clear(); // Clear the list
    }
    protected override IEnumerator SpawnHitBox()
    {
        yield return new WaitForSeconds(attackStartup);

        AttackAnimationDirection animDirection = GetAnimAttackingDirection();
        Vector2 spawnDirection = animDirection.difference;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calculate the spawn position of the projectile
            Vector3 spawnPosition = GetClosestPointEnemy(transform.position, closestTargetPosition, 0.1f) + (Vector3)(spawnDirection * (projectileSpacing * i));
            // Instantiate the projectile
            GameObject projectile = Instantiate(prefabHitbox, spawnPosition, Quaternion.identity, gameObject.transform);

            spawnedProjectiles.Add(projectile);

            // Wait for 0.15 seconds before spawning the next projectile
            yield return new WaitForSeconds(0.15f);
        }
    }

    public Vector3 GetClosestPointEnemy(Vector3 casterPosition, Vector3 target, float distanceFromCaster = 10f)
    {
        Vector2 direction = target - casterPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Debug.Log("Angle: " + angle);
        // Calculate the Y component of the closest point based on the angle
        yOffset = 0f; 
        xOffset = 0f;
        if (angle > -170f && angle < -10f)
        {

            float angleRatio = (Mathf.Abs(angle)) / 180f; // Normalize angle to a 0-1 range

            if (angleRatio < 0.5f)
            {
                yOffset = -1f * (angleRatio / 0.5f);
            }
            else
            {
                yOffset = -1f * (1 - (angleRatio - 0.5f) / 0.5f);
            }
            
        }

        float xLeftThreshold = -125f;
        float xRightThreshold = 145f;
        float xRange = Mathf.Abs(xLeftThreshold) + Mathf.Abs(xRightThreshold);
        if (angle < xLeftThreshold || angle > xRightThreshold)
        {
            float angleRatio;
            if (angle < 0)
            {
                angleRatio = (Mathf.Abs(angle) - Mathf.Abs(xLeftThreshold)) / xRange;
            }
            else
            {
                angleRatio = (angle - xRightThreshold) / xRange;
            }

            xOffset = -4f * angleRatio;
        }
        else if (angle > -35f && angle < 35f)
        {
            float angleRatio;
            if (angle < 0)
            {
                angleRatio = (Mathf.Abs(angle) - 35f) / 70f;
            }
            else
            {
                angleRatio = (angle - 35f) / 70f;
            }

            xOffset = -4f * angleRatio;
        }
        else
        {
            xOffset = 0f;
        }

        Vector3 closestPoint = casterPosition + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) + xOffset, Mathf.Sin(angle * Mathf.Deg2Rad) + yOffset, 0);

        return closestPoint;
    }
}


