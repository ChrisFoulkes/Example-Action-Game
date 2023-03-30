using EventCallbacks;
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

    public MeleeAbility(MeleeData aData, AbilityCasterContext caster) : base(aData)
    {
        meleeData = new ActiveMeleeData(aData.meleeDamage, aData.castTime, aData.critChance);

        statusEffects = aData.effects;
        meleeAttack = aData.meleePrefab;
        CastTime = aData.castTime;

        this._caster = (PlayerCasterContext)caster;

        AbilityUpgradeHandler = new MeleeUpgradeHandler(this);
    }


    public override void CastAbility()
    {
        if (CastTime > Cooldown) { AdjustCooldown(CastTime); }

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

    public void OnHit(Collider2D collision, MeleeAttack attack)
    {
        IDamage damageController = collision.GetComponentInParent<IDamage>();

        float randomNumber = Random.Range(0f, 1f);

        float damageValue = Mathf.RoundToInt(meleeData.meleeDamage.CalculateModifiedValue(_caster.CharacterStatsController));
        bool isCrit = false;
        if (randomNumber < meleeData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            isCrit = true;
            damageValue *= 2;
        }

        damageController.ApplyDamage(new DamageInfo(damageValue, isCrit, FloatingColourType.Generic, hitStun, knockbackData), _caster);


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
