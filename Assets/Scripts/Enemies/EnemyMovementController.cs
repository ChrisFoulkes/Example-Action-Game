using EventCallbacks;
using Pathfinding;
using System.Collections;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public class MovementAnimationState
{
    public Vector3 desiredDirection;
    public bool moving = false;

    public MovementAnimationState(Vector3 desiredVelocity, bool isMoving)
    {
        desiredDirection = desiredVelocity;
        moving = isMoving;
    }
}

public class EnemyMovementController : MonoBehaviour, IMovement
{
    private AIPath _aiPath;
    private Rigidbody2D _rb2D;
    private IDamage _damageController;

    public bool isKnockbackAble = true;
    private float baseMovementSpeed = 3f;

    void Awake()
    {
        _aiPath = GetComponentInParent<AIPath>();
        _rb2D = GetComponentInParent<Rigidbody2D>();
        _damageController = GetComponent<IDamage>();
    }


    public void Initialize(float initialMovement)
    {
        baseMovementSpeed = initialMovement;
        SetInitialMoveSpeed();
    }

    public void OnEnable()
    {
        _damageController.AddDamageListener(OnDamageEvent);
    }

    public void OnDisable()
    {

        _damageController.RemoveDamageListener(OnDamageEvent);
    }

    public MovementAnimationState GetMovementAnimState()
    {
        bool isMoving = (Mathf.Abs(_rb2D.velocity.x) > 0 || Mathf.Abs(_aiPath.velocity.y) > 0);
        return new MovementAnimationState(_aiPath.desiredVelocity, isMoving);
    }

    void SetInitialMoveSpeed()
    {
        _aiPath.maxSpeed = baseMovementSpeed;
    }

    public void CanMove(bool canMove)
    {
        if (!canMove)
        {
            _aiPath.maxSpeed = 0f;
        }
        else
        {
            _aiPath.maxSpeed = baseMovementSpeed;
        }
    }

    public void ChangeSpeed(float amount)
    {
        Debug.Log("Slowing Enemies not implemented!");
    }


    public void HandleKnockback(Vector2 knockbackDirection, Vector2 knockbackForce, float delay)
    {
        if (isKnockbackAble)
        {
            if (_rb2D != null)
            {
                StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce, delay));
            }
            else
            {
                Debug.LogWarning("Rigidbody2D component not found on the enemy.");
            }
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection, Vector2 knockbackForce, float duration)
    {
        _aiPath.canMove = false;
        CanMove(false);


        if (_rb2D != null)
        {
            _rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(duration);
        _rb2D.velocity = Vector2.zero;
        CanMove(true);
        _aiPath.canMove = true;
    }

    private void OnDamageEvent(GameEvent dEvent)
    {
        EnemyDamageEvent damageEvent = (EnemyDamageEvent)dEvent;
        KnockbackData knockback = damageEvent.DamageInfo.Knockback;

        if (knockback != null)
        {
            if (knockback.shouldKnockback)
            {
                Vector2 knockbackDirection;

                if (knockback.knockbackFromEffect)
                {
                    knockbackDirection = (transform.position - damageEvent.DamageInfo.EffectPosition).normalized;
                }
                else
                {
                    knockbackDirection = (transform.position - damageEvent.Caster.transform.position).normalized;
                }


                HandleKnockback(knockbackDirection, knockback.Force, knockback.Duration);
            }
        }
    }
}
