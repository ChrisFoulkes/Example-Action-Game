using EventCallbacks;
using System.Collections;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private IDeath _deathController;
    private IMovement _movementController;
    public EnemyAnimationController _animController;
    private EnemyAttackController _attackController;
    private IDamage _damageController;
    private bool _isHurt = false; 
    private float _hitStunDuration = 0f;

    void Awake()
    {
        _attackController = GetComponent<EnemyAttackController>();
        _movementController = GetComponent<IMovement>();
        _deathController = GetComponent<IDeath>();
        _damageController = GetComponent<IDamage>();

    }

    public void OnEnable()
    {
        _damageController.AddDamageListener(OnDamageEvent);
    }

    public void OnDisable()
    {

        _damageController.RemoveDamageListener(OnDamageEvent);
    }

    void FixedUpdate()
    {
        _movementController.CanMove(false);

        if (IsDead())
        {
            _animController.SetState(EnemyAnimationController.EnemyState.Die);
            return;
        }

        if (IsAttacking())
        {
            _animController.SetState(EnemyAnimationController.EnemyState.Attack);
        }
        else if (IsHurt())
        {
            _animController.SetState(EnemyAnimationController.EnemyState.Hurt);
           
        }
        else if (IsMoving())
        {
            _animController.SetState(EnemyAnimationController.EnemyState.Move);
            _movementController.CanMove(true);
        }
        else
        {
            //enemies currently can't idle
            _animController.SetState(EnemyAnimationController.EnemyState.Idle);
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
        if (_attackController.CanAttack())
        {
            _attackController.StartEnemyAttack();
            return true;
        }
        if (_attackController.IsAttacking())
        {
            return true;
        }
        return false;
    }

    bool IsDead()
    {
        return _deathController.IsDead();
    }

    bool IsHurt()
    {
        return _isHurt;
    }

    public void SetHurtState(bool value)
    {
        _isHurt = value;
    }
    public IEnumerator ResetHurtState(float duration)
    {
        _hitStunDuration += duration;

        while (_hitStunDuration > 0)
        {
            _hitStunDuration -= Time.deltaTime;
            yield return null;
        }

        SetHurtState(false);
        _animController.SetState(EnemyAnimationController.EnemyState.Idle);
    }

    private void OnDamageEvent(GameEvent dEvent)
    {
        EnemyDamageEvent damageEvent = (EnemyDamageEvent)dEvent;

        float hitStun = damageEvent.DamageInfo.HitStun;

        if (hitStun > 0)
        {
            SetHurtState(true);
            StopCoroutine(nameof(ResetHurtState));
            StartCoroutine(ResetHurtState(damageEvent.DamageInfo.HitStun));
        }
    }
}