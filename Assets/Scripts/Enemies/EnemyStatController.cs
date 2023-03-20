using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAnimationController;

[Serializable]
public class EnemyStats
{
    [Header("EnemyStats")]
    public float health;
    public float healthGrowthFactor;
    public float attackDamage;
    public float damageGrowthFactor;
    public float movementSpeed;
    public float movementSpeedGrowthFactor;
}

public class EnemyStatController : MonoBehaviour
{
    public EnemyStats enemyStats;

    private HealthController healthController;
    private EnemyAttackController attackController;
    private EnemyMovementController movementController;
    public Transform scaleParentTransform;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
        attackController = GetComponent<EnemyAttackController>();
        movementController = GetComponent<EnemyMovementController>();
    }

    public void InitializeControllers(int waveCount)
    {
        healthController.Initialize(enemyStats.health + (enemyStats.health * (enemyStats.healthGrowthFactor * waveCount)));
        attackController.Initialize(enemyStats.attackDamage + (enemyStats.attackDamage * (enemyStats.damageGrowthFactor * waveCount)));
        movementController.Initialize(enemyStats.movementSpeed + (enemyStats.movementSpeed  * (enemyStats.movementSpeedGrowthFactor * waveCount)));

        scaleParentTransform.localScale = new Vector3(1f + (waveCount * 0.05f), 1f + (waveCount * 0.05f), 0);
    }
}
