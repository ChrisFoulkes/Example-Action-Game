using EventCallbacks;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
                    parentAbility.meleeData.meleeDamage.baseValue += Mathf.RoundToInt(meleeUpgradeEffect.amount);
                    break;
                case MeleeUpgradeTypes.meleeCastSpeed:
                    parentAbility.castTime = Mathf.Max(parentAbility.castTime + meleeUpgradeEffect.amount, (parentAbility.meleeData.originalCastTime * 0.70f));
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
public class MeleeAbility : Ability
{
    public ActiveMeleeData meleeData;

    private MeleeAttack meleeAttack;
    private Transform caster;
    private Transform projectileSpawnPoint;
    public CharacterStatsController characterStatsController;

    private List<StatusEffect> statusEffects;

    public MeleeAbility(MeleeData aData, Transform casterTransform, Transform projectileSpawnPos) : base(aData)
    {
        meleeData = new ActiveMeleeData(aData.meleeDamage, aData.castTime, aData.critChance);
        statusEffects = aData.effects;
        meleeAttack = aData.meleePrefab;
        castTime = aData.castTime;

        caster = casterTransform;
        projectileSpawnPoint = projectileSpawnPos;
        upgradeHandler = new MeleeUpgradeHandler(this);
        //need to update the way we handle passing caster data this is dumb
        characterStatsController = caster.gameObject.GetComponent<CharacterStatsController>();
    }


    public override void CastAbility()
    {
        if (castTime > cooldown){ adjustCooldowm(castTime); }

        GameObject melee = Object.Instantiate(meleeAttack.gameObject, projectileSpawnPoint.position, SetTheFiringRotation());

        melee.GetComponent<MeleeAttack>().Initialize(this);

        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        EventManager.Raise(stopMovementEvent); 

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.getFacingDirection(caster.position), castTime);
        EventManager.Raise(setDirectionEvent);
    }
    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        upgradeHandler.ApplyUpgrade(upgradeEffect);
    }

    Quaternion SetTheFiringRotation()
    {
        Vector2 lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.position).normalized;

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
        IHealth hitHealth = collision.GetComponentInParent<IHealth>();

        float randomNumber = UnityEngine.Random.Range(0f, 1f);

        float damageValue = Mathf.FloorToInt(meleeData.meleeDamage.CalculateModifiedValue(characterStatsController));

        Debug.Log(meleeData.critChance.CalculateModifiedValue(characterStatsController));
        Debug.Log(randomNumber < meleeData.critChance.CalculateModifiedValue(characterStatsController));

        if (randomNumber < meleeData.critChance.CalculateModifiedValue(characterStatsController))
        {
            hitHealth.ChangeHealth((damageValue * 2), true);
        }
        else
        {

            hitHealth.ChangeHealth(damageValue);
        }

        if (statusEffects.Count > 0)
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in statusEffects)
            {
                effect.ApplyEffect(statusController, caster.gameObject);
            }
        }

        // relies on hard code data in the attack script needs to be shifted into data and maybe even some onhit list of effects 
        if (attack.shouldKnockback)
        {
                EnemyMovementController movementController = collision.GetComponentInParent<EnemyMovementController>();

                if (movementController != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - caster.position).normalized;
                    
                    movementController.HandleKnockback(knockbackDirection, attack.knockbackForce, 0.2f);
                }
        }
    }
}
