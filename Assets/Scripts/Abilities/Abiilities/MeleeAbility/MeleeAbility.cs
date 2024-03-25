using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MeleeUpgradeHandler : UpgradeHandler
{
    private MeleeAbility parentAbility;

    public MeleeUpgradeHandler(MeleeAbility parentAbility)
    {
        this.parentAbility = parentAbility;
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        if (upgradeEffect is MeleeUpgradeEffect meleeUpgradeEffect)
        {
            switch (meleeUpgradeEffect.upgradeType)
            {
                case MeleeUpgradeTypes.meleeDamage:
                    parentAbility.MeleeData.meleeDamage.baseValue += Mathf.RoundToInt(meleeUpgradeEffect.amount);
                    break;
                case MeleeUpgradeTypes.meleeCastSpeed:
                    parentAbility.CastTime = Mathf.Max(parentAbility.CastTime + meleeUpgradeEffect.amount, (parentAbility.MeleeData.originalCastTime * 0.70f));
                    break;
            }
        }
    }
}

public class ActiveMeleeData
{
    public StatAssociation meleeDamage;
    public StatAssociation critChance;
    public StatAssociation baseAoe;
    public float originalCastTime;

    public ActiveMeleeData(StatAssociation meleeDamage, float castTime, StatAssociation critChance, StatAssociation baseAoe)
    {
        this.critChance = new StatAssociation(critChance);
        this.critChance.associatedStats = critChance.associatedStats;
        this.meleeDamage = new StatAssociation(meleeDamage);
        this.meleeDamage.associatedStats = meleeDamage.associatedStats;
        this.baseAoe = new StatAssociation(baseAoe);
        this.baseAoe.associatedStats = baseAoe.associatedStats;
        this.originalCastTime = castTime;
    }

}
public class MeleeAbility : HitAbility
{
    public ActiveMeleeData MeleeData;

    private MeleeAttack _meleeAttack;
    PlayerCasterContext _caster;


    private List<StatusEffect> _statusEffects;
    private List<ActiveBuffData> _buffEffects = new List<ActiveBuffData>();

    public MeleeAbility(MeleeData aData, CasterContext caster) : base(aData)
    {
        MeleeData = new ActiveMeleeData(aData.meleeDamage, aData.castTime, aData.critChance, aData.baseAoe);

        _statusEffects = aData.statusEffects;

        foreach (BuffData data in aData.buffEffects)
        {
            _buffEffects.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID, data.isStackable, data.maxStacks));
        }

        _meleeAttack = aData.meleePrefab;
        CastTime = aData.castTime;

        _caster = (PlayerCasterContext)caster;

        AbilityUpgradeHandler = new MeleeUpgradeHandler(this);
    }

    protected override void OnCast()
    {

        AbilityScale = MeleeData.baseAoe.CalculateModifiedValue(_caster.CharacterStatsController);

        GameObject melee = Object.Instantiate(_meleeAttack.gameObject, _caster.AttackSpawnPos.position, AbilityUtils.GetMeleeFiringRotation(_caster.transform.position));
       // GameObject melee = Object.Instantiate(_meleeAttack.gameObject, _caster.AttackSpawnPos.position, _meleeAttack.gameObject.transform.rotation);

        melee.GetComponent<MeleeAttack>().Initialize(this);
    }


    //Again collision isnt perfect need a way of getting an enemy Context ? onHit query the target perhaps
    public override void OnHit(TargetContext target, Attack attack)
    {
        float damageValue = Mathf.RoundToInt(MeleeData.meleeDamage.CalculateModifiedValue(_caster.CharacterStatsController));

        //Not super happy with this sending down the calculatedValue need to unify critical hits across all abilities to make the calculation inside ability.cs
        bool isCrit = false;
        if (IsCriticalHit(MeleeData.critChance.CalculateModifiedValue(_caster.CharacterStatsController)))
        {
            isCrit = true;
            damageValue *= 2;
        }

        target.DamageController.ApplyDamage(new DamageInfo(damageValue, isCrit, FloatingColourType.Generic, hitStun, knockbackData), _caster);

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
