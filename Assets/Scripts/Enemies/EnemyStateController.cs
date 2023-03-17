using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public IHealth healthController;
    public IDeath deathController;
    public IMovement movementController;
    public EnemyAnimationController animController;
    public EnemyAttackController attackController;

    void Start()
    {
        // Assuming the EnemyHealthController and EnemyMovementController are attached to the same GameObject
        healthController = GetComponent<IHealth>();
        movementController = GetComponent<IMovement>();
        deathController = GetComponent<IDeath>();
    }

    void FixedUpdate()
    {
        movementController.CanMove(false);
        if (deathController.IsDead())
        {
            animController.SetState(EnemyAnimationController.EnemyState.Die);
            return;
        }

        if (IsAttacking())
        {
            animController.SetState(EnemyAnimationController.EnemyState.Attack);
        }
        else if (IsMoving())
        {
            animController.SetState(EnemyAnimationController.EnemyState.Move);
            movementController.CanMove(true);
        }
        else
        {
            animController.SetState(EnemyAnimationController.EnemyState.Idle);
        }
    }

    bool IsMoving()
    {
        // Implement logic to determine if the enemy is moving.
        // For example, you can check the distance to the player or the movement direction.
        return true;
    }

    bool IsAttacking()
    {
        if (attackController.CanAttack()) 
        {
            attackController.StartEnemyAttack();
            return true;
        }
        if (attackController.IsAttacking())
        {
            return true;
        }
        // Implement logic to determine if the enemy is attacking.
        // For example, you can check the distance to the player or an attack timer.
        return false;
    }
}