using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Hurt,
        Die
    }

    public Animator animator;

    [SerializeField] private EnemyState currentState;
    public EnemyAttackController attackController;
    public EnemyMovementController movementController;

    public Enemy deathController;

    public bool StateChanged = false;

    void Awake()
    {
        animator = GetComponent<Animator>();

        //Set Default State
        currentState = EnemyState.Idle;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
        UpdateAnimation();
    }

    public void FixedUpdate()
    {
        if (currentState == EnemyState.Move)
        {
            SetMovementDirection();
        }
    }

    public void CompleteDeathAnimation()
    {
        deathController.CompleteDeath();
    }

    void UpdateAnimation()
    {
        animator.SetBool("Moving", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsHurt", false);

        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Move:
                UpdateMoveState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.Hurt:
                UpdateHurtState();
                break;
            case EnemyState.Die:
                UpdateDieState();
                break;
        }

    }

    private void UpdateIdleState()
    {
        animator.SetBool("Moving", false);
    }
    private void UpdateHurtState()
    {
        animator.SetBool("IsHurt", true);
    }
    private void UpdateMoveState()
    {
        animator.SetBool("Moving", true);
    }

    private void UpdateAttackState()
    {
        SetAttackingDirection();
        animator.SetBool("IsAttacking", true);
    }

    private void UpdateDieState()
    {
        animator.SetBool("IsDead", true);
    }


    void SetMovementDirection()
    {
        MovementAnimationState moveState = movementController.GetMovementAnimState();
        animator.SetFloat("DirectionY", moveState.desiredDirection.y);
        animator.SetFloat("DirectionX", moveState.desiredDirection.x);
        animator.SetBool("Moving", moveState.moving);
    }

    public void SetAttackingDirection()
    {
        AttackAnimationDirection attackDirection = attackController.GetAnimAttackingDirection();
        animator.SetBool("AttackingY", attackDirection.yAxis);
        animator.SetFloat("AttackingDirectionY", attackDirection.difference.y);
        animator.SetFloat("AttackingDirectionX", attackDirection.difference.x);
    }

}
