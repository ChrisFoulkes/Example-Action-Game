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
    private List<ActiveBuffData> _buffEffects = new List<ActiveBuffData>();

    private Vector2 offset = new Vector2(-3f, 2f);

    public MouseTargetAbility(MouseTargetData aData, CasterContext caster) : base(aData)
    {
        _caster = (PlayerCasterContext)caster;
        MTData = new ActiveMTData(aData.Damage, aData.critChance);
        _statusEffects = aData.statusEffects; 
        
        foreach (BuffData data in aData.buffEffects)
        {
            _buffEffects.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID, data.isStackable, data.maxStacks));
        }

        _targetAttack = aData.attackPrefab;
    }
    protected override void OnCast()
    {
        var direction = AbilityUtils.GetLeftOrRightDirection(_caster.transform.position);
        //Temporary needs updating to be more generic
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
    }



    public override void OnHit(TargetContext target, Attack attack)
    {
        float randomNumber = Random.Range(0f, 1f);

        float damageValue = Mathf.RoundToInt(MTData.abilityDamage.CalculateModifiedValue(_caster.CharacterStatsController));
        bool isCrit = false;
        if (randomNumber < MTData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            isCrit = true;
            damageValue *= 2;
        }

        target.DamageController.ApplyDamage(new DamageInfo(damageValue, isCrit, FloatingColourType.Generic, hitStun, attack.transform.position,  knockbackData), _caster);

        ApplyBuffEffects();
        ApplyStatusEffects(target);
    }

    // need to unify these effects into the HitAbility to remove duplicated code across melee, projectile and mousetarget ability.
    public void ApplyBuffEffects()
    {
        foreach (ActiveBuffData buff in _buffEffects)
        {
            _caster.BuffController.ApplyBuff(buff);
        }
    }


    public void ApplyStatusEffects(TargetContext target)
    {

        if (_statusEffects.Count > 0)
        {

            foreach (var effect in _statusEffects)
            {
                effect.ApplyEffect(target.StatusController, _caster);
            }
        }
    }
}
