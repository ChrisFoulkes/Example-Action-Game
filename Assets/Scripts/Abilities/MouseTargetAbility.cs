using EventCallbacks;
using System.Collections.Generic;
using UnityEngine;

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

public class MouseTargetAbility : HitAbility
{
    private PlayerCasterContext _caster;
    private ActiveMTData MTData;
    private readonly GameObject _targetAttack;
    private List<StatusEffect> _statusEffects;
    private Vector2 offset = new Vector2(-3f, 2f);

    public MouseTargetAbility(MouseTargetData aData, AbilityCasterContext caster) : base(aData)
    {
        _caster = (PlayerCasterContext)caster;
        MTData = new ActiveMTData(aData.Damage, aData.critChance);
        _statusEffects = aData.effects;
        _targetAttack = aData.attackPrefab;
    }
    public override void CastAbility()
    {
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
        stopMovementEvent.duration = CastTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(_caster.transform.position), CastTime);
        EventManager.Raise(setDirectionEvent);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
    }

    public void OnHit(Collider2D collision, TargetAttack attack)
    {
        IDamage damageController = collision.GetComponentInParent<IDamage>();
        float randomNumber = Random.Range(0f, 1f);

        float damageValue = Mathf.RoundToInt(MTData.abilityDamage.CalculateModifiedValue(_caster.CharacterStatsController));
        bool isCrit = false;
        if (randomNumber < MTData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            isCrit = true;
            damageValue *= 2;
        }

        damageController.ApplyDamage(new DamageInfo(damageValue, isCrit, FloatingColourType.Generic, hitStun, attack.transform.position,  knockbackData), _caster);

        if (_statusEffects.Count > 0)
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in _statusEffects)
            {
                effect.ApplyEffect(statusController, _caster);
            }
        }
    }
}
