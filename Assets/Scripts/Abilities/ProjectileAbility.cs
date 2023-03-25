using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeHandler 
{
    public abstract void ApplyUpgrade(UpgradeEffect upgradeEffect);
}


public class ProjectileUpgradeHandler: UpgradeHandler
{
    private ProjectileAbility parentAbility;

    public ProjectileUpgradeHandler(ProjectileAbility parentAbility)
    {
        this.parentAbility = parentAbility;
    }

    public  override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        if (upgradeEffect is ProjectileUpgradeEffect projectileUpgradeEffect)
        {
            switch (projectileUpgradeEffect.upgradeType)
            {
                case ProjectileUpgradeTypes.projectileDamage:
                    parentAbility.projData.projectileDamage += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileCount:
                    parentAbility.projData.projectileCount += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileArc:
                    parentAbility.projData.firingArc = Mathf.Clamp(parentAbility.projData.firingArc + projectileUpgradeEffect.amount, 0, 360);
                    break;
            }
        }
    }
}

public class ActiveProjectileData 
{
    //projectile activity --
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    public StatAssociation critChance;
    //projectile firing -- 
    public int projectileCount;
    public float firingArc;
    public float castTime;
    public float distanceFromCaster;

    public ActiveProjectileData(float projectileSpeed, float projectileLifetime, int projectileDamage, int projectileCount, float firingArc, float castTime, float distanceFromCaster, StatAssociation critChance)
    {
        this.projectileSpeed = projectileSpeed;
        this.projectileLifetime = projectileLifetime;
        this.projectileDamage = projectileDamage;
        this.projectileCount = projectileCount;
        this.firingArc = firingArc;
        this.castTime = castTime;
        this.distanceFromCaster = distanceFromCaster;
        this.critChance = new StatAssociation(critChance);
    }
}

public class ProjectileAbility : Ability
{
    public  ActiveProjectileData projData;

    private readonly ProjectileAttack projectileAttack;
    private readonly Transform caster;
    private readonly Transform projectileSpawnPoint;
    public CharacterStatsController CharacterStatsController;

    private List<StatusEffect> statusEffects;

    public ProjectileAbility(ProjectileData aData, Transform casterTransform, Transform projectileSpawnPos): base(aData)
    {
        projData = new ActiveProjectileData(aData.projectileSpeed, aData.projectileLifetime, aData.projectileDamage, aData.ProjectileCount, aData.firingArc, aData.castTime, aData.distanceFromCaster,  aData.critChance);

        statusEffects = aData.effects;
        projectileAttack = aData.projectilePrefab;
        caster = casterTransform;
        projectileSpawnPoint = projectileSpawnPos;
        upgradeHandler = new ProjectileUpgradeHandler(this);
        //need to update the way we handle passing caster data this is dumb
        CharacterStatsController = caster.gameObject.GetComponent<CharacterStatsController>();
    }

        public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
        {
            upgradeHandler.ApplyUpgrade(upgradeEffect);
        }

    public override void CastAbility()
    {
        if (projData.castTime > cooldown) { adjustCooldowm(projData.castTime); }

        for (int i = 0; i < projData.projectileCount; i++)
        {
            //ignore firing arc for singular projectiles 
            if (projData.projectileCount == 1)
            {
                projData.firingArc = 0;
            }

            SetTheFiringRotation(projData.firingArc, AbilityUtils.getFiringArcIncrement(i, projData.firingArc, projData.projectileCount));

            GameObject projectile = Object.Instantiate(projectileAttack.gameObject, AbilityUtils.GetClosestPointToMouse(caster.position, projectileSpawnPoint.position, projData.distanceFromCaster), projectileSpawnPoint.rotation);
            projectile.GetComponent<ProjectileAttack>().Initialize(this);
        }

        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = projData.castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.getFacingDirection(caster.position), projData.castTime);
        EventManager.Raise(setDirectionEvent);
    }


    void SetTheFiringRotation(float arc, float increment)
    {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.position;
        float lookAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - (arc / 2);

        projectileSpawnPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f + increment);
    }

    public void OnHit(Collider2D collision, ProjectileAttack attack) 
    {
        IHealth hitHealth = collision.GetComponentInParent<IHealth>();

        float randomNumber = Random.Range(0, 1f);

        if (randomNumber < projData.critChance.CalculateModifiedValue(CharacterStatsController))
        {
            hitHealth.ChangeHealth((projData.projectileDamage * 2), true);
        }
        else 
        {

            hitHealth.ChangeHealth(projData.projectileDamage);
        }

        if(statusEffects.Count> 0) 
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in statusEffects) 
            {
                effect.ApplyEffect(statusController, caster.gameObject);
            }
        }
    }
}
