using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Die
    }

    public Animator animator;

    [SerializeField]private EnemyState currentState;
    public EnemyAttackController attackController;
    public EnemyMovementController movementController;
    public GameObject parent;

    public bool stateChanged = false;   

    void Awake()
    {
        animator = GetComponent<Animator>();

        //Set Default State
        currentState = EnemyState.Idle;
    }

    public void SetState(EnemyState newState)
    {
        stateChanged = (currentState != newState);

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
        GameObject.Destroy(parent);
    }

    void UpdateAnimation()
    {
        animator.SetBool("Moving", false);
        animator.SetBool("IsAttacking", false);

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("Moving", false);
                break;
            case EnemyState.Move:
                animator.SetBool("Moving", true);
                break;
            case EnemyState.Attack:
                if (stateChanged)
                {
                    SetAttackingDirection();
                }
                animator.SetBool("IsAttacking", true);
                break;
            case EnemyState.Die:
                animator.SetBool("IsDead", true);
                break;
        }
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
        stateChanged = false;
        AttackAnimationDirection attackDirection = attackController.GetAnimAttackingDirection();
        animator.SetBool("AttackingY", attackDirection.yAxis);
        animator.SetFloat("AttackingDirectionY", attackDirection.difference.y);
        animator.SetFloat("AttackingDirectionX", attackDirection.difference.x);
    }

}
