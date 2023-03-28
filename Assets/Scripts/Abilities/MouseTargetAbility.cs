using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Pathfinding.Util.RetainedGizmos;

public class ActiveMTData
{
    public StatAssociation abilityDamage;
    public StatAssociation critChance;


    public ActiveMTData(StatAssociation Damage, StatAssociation critChance)
    {
        this.abilityDamage = new StatAssociation(Damage);
        this.critChance = new StatAssociation(critChance);
    }
}

public class MouseTargetAbility : Ability
{
    private AbilityContext _caster;
    private ActiveMTData MTData;
    private readonly GameObject _targetAttack;
    private List<StatusEffect> _statusEffects;
    private Vector2 offset = new Vector2(-3f, 2f);

    public MouseTargetAbility(MouseTargetData aData, AbilityContext caster) : base(aData)
    {
        _caster = caster;
        MTData = new ActiveMTData(aData.Damage, aData.critChance);
        _statusEffects = aData.effects;
        _targetAttack = aData.attackPrefab;
    }
    public override void CastAbility()
    {
        if (castTime > cooldown) { AdjustCooldown(castTime); }

        var direction = AbilityUtils.GetLeftOrRightDirection(_caster.transform.position);
        if (direction > 0)
        {
            offset.x = -3f;
        }
        else 
        {
            offset.x = 3f;
        }


        GameObject targetAbilityPrefab = Object.Instantiate(_targetAttack, AbilityUtils.GetOffsetMousePosition(offset), _targetAttack.transform.rotation);
        targetAbilityPrefab.transform.localScale = new Vector3(direction, 1, 1);
        targetAbilityPrefab.GetComponent<TargetAttack>().Initialize(this, AbilityUtils.GetMousePosition());


        // Currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(_caster.transform.position), castTime);
        EventManager.Raise(setDirectionEvent);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    { 
    }

    public void OnHit(Collider2D collision, TargetAttack attack)
    {
        IHealth hitHealth = collision.GetComponentInParent<IHealth>();
        float randomNumber = Random.Range(0f, 1f);

        float damageValue = Mathf.RoundToInt(MTData.abilityDamage.CalculateModifiedValue(_caster.CharacterStatsController));

        if (randomNumber < MTData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            hitHealth.ChangeHealth(damageValue * 2, true);
        }
        else
        {

            hitHealth.ChangeHealth(damageValue);
        }


        if (_statusEffects.Count > 0)
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in _statusEffects)
            {
                effect.ApplyEffect(statusController, _caster);
            }
        }

        EnemyMovementController movementController = collision.GetComponentInParent<EnemyMovementController>();

        Vector2 knockbackDirection = (collision.transform.position - attack.transform.position).normalized;

        movementController.HandleKnockback(knockbackDirection, new Vector2(8f, 8f), 0.3f);
    

    }
}
