using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
                    parentAbility.meleeData.meleeDamage.baseValue += Mathf.RoundToInt(meleeUpgradeEffect.amount);
                    break;
                case MeleeUpgradeTypes.meleeCastSpeed:
                    parentAbility.CastTime = Mathf.Max(parentAbility.CastTime + meleeUpgradeEffect.amount, (parentAbility.meleeData.originalCastTime * 0.70f));
                    break;
            }
        }
    }
}

public class ActiveMeleeData
{
    public StatAssociation meleeDamage;
    public StatAssociation critChance;
    public float originalCastTime;

    public ActiveMeleeData(StatAssociation meleeDamage, float castTime, StatAssociation critChance)
    {
        this.critChance = new StatAssociation(critChance);
        this.critChance.associatedStats = critChance.associatedStats;
        this.meleeDamage = new StatAssociation(meleeDamage);
        this.meleeDamage.associatedStats = meleeDamage.associatedStats;
        this.originalCastTime = castTime;
    }

}
public class MeleeAbility : HitAbility
{
    public ActiveMeleeData meleeData;

    private MeleeAttack meleeAttack;
    PlayerCasterContext _caster;

    private List<StatusEffect> statusEffects;
    private List<ActiveBuffData> buffEffects = new List<ActiveBuffData>();
    public MeleeAbility(MeleeData aData, AbilityCasterContext caster) : base(aData)
    {
        meleeData = new ActiveMeleeData(aData.meleeDamage, aData.castTime, aData.critChance);

        statusEffects = aData.statusEffects;

        foreach (BuffData data in aData.buffEffects)
        {
            buffEffects.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID));
        }

        meleeAttack = aData.meleePrefab;
        CastTime = aData.castTime;

        this._caster = (PlayerCasterContext)caster;

        AbilityUpgradeHandler = new MeleeUpgradeHandler(this);
    }


    public override void CastAbility()
    {

        GameObject melee = Object.Instantiate(meleeAttack.gameObject, _caster.ProjectileSpawnPos.position, SetTheFiringRotation());

        melee.GetComponent<MeleeAttack>().Initialize(this);

        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = CastTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(_caster.transform.position), CastTime);
        EventManager.Raise(setDirectionEvent);
    }
    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        AbilityUpgradeHandler.ApplyUpgrade(upgradeEffect);
    }

    Quaternion SetTheFiringRotation()
    {
        Vector2 lookDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _caster.transform.position).normalized;

        float angle;
        if (lookDirection.y > 0 && Mathf.Abs(lookDirection.x) < lookDirection.y)
        {
            angle = 0;
        }
        else if (lookDirection.y < 0 && Mathf.Abs(lookDirection.x) < -lookDirection.y)
        {
            angle = 180;
        }
        else if (lookDirection.x > 0)
        {
            angle = -90;
        }
        else
        {
            angle = 90;
        }

        return Quaternion.Euler(0, 0, angle);
    }

    //Again collision isnt perfect need a way of getting an enemy Context ? onHit query the target perhaps
    public void OnHit(Collider2D collision, MeleeAttack attack)
    {
        IDamage damageController = collision.GetComponentInParent<IDamage>();

       
        float damageValue = Mathf.RoundToInt(meleeData.meleeDamage.CalculateModifiedValue(_caster.CharacterStatsController));

        //Not super happy with this sending down the calculatedValue need to unify critical hits across all abilities to make the calculation inside ability.cs
        bool isCrit = false;
        if (IsCriticalHit(meleeData.critChance.CalculateModifiedValue(_caster.CharacterStatsController)))
        {
            isCrit = true;
            damageValue *= 2;
        }

        damageController.ApplyDamage(new DamageInfo(damageValue, isCrit, FloatingColourType.Generic, hitStun, knockbackData), _caster);

        ApplyBuffEffects();
        ApplyStatusEffects(collision);
        SpawnHitEffect(collision);
    }

    public void SpawnHitEffect(Collider2D collision)
    {
        Transform enemyTransform = collision.transform;
        Vector2 collisionPoint = collision.ClosestPoint(enemyTransform.position);
        HitEffectSpawner.Instance.SpawnHitEffect(collisionPoint, enemyTransform);
    }
    public void ApplyBuffEffects()
    {
        foreach (ActiveBuffData buff in buffEffects)
        {
            _caster.BuffController.ApplyBuff(buff);
        }
    }


    public void ApplyStatusEffects(Collider2D collision) 
    {

        if (statusEffects.Count > 0)
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in statusEffects)
            {
                effect.ApplyEffect(statusController, _caster);
            }
        }
    }

}
