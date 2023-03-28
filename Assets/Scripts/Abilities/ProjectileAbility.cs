using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileUpgradeHandler : UpgradeHandler
{
    private readonly ProjectileAbility _parentAbility;

    public ProjectileUpgradeHandler(ProjectileAbility parentAbility)
    {
        _parentAbility = parentAbility;
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        if (upgradeEffect is ProjectileUpgradeEffect projectileUpgradeEffect)
        {
            switch (projectileUpgradeEffect.upgradeType)
            {
                case ProjectileUpgradeTypes.projectileDamage:
                    _parentAbility.ProjData.projectileDamage += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileCount:
                    _parentAbility.ProjData.projectileCount += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileArc:
                    _parentAbility.ProjData.firingArc = Mathf.Clamp(_parentAbility.ProjData.firingArc + projectileUpgradeEffect.amount, 0, 360);
                    break;
            }
        }
    }
}

public class ActiveProjectileData
{
    // Projectile activity
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    public StatAssociation critChance;

    // Projectile firing
    public int projectileCount;
    public float firingArc;
    public float distanceFromCaster;

    public ActiveProjectileData(float projectileSpeed, float projectileLifetime, int projectileDamage, int projectileCount, float firingArc, float distanceFromCaster, StatAssociation critChance)
    {
        this.projectileSpeed = projectileSpeed;
        this.projectileLifetime = projectileLifetime;
        this.projectileDamage = projectileDamage;
        this.projectileCount = projectileCount;
        this.firingArc = firingArc;
        this.distanceFromCaster = distanceFromCaster;
        this.critChance = new StatAssociation(critChance);
    }
}

public class ProjectileAbility : Ability
{
    private AbilityContext _caster;
    public ActiveProjectileData ProjData { get; }
    private readonly ProjectileAttack _projectileAttack;
    private List<StatusEffect> _statusEffects;

    public ProjectileAbility(ProjectileData aData, AbilityContext caster) : base(aData)
    {
        _caster = caster;
        ProjData = new ActiveProjectileData(aData.projectileSpeed, aData.projectileLifetime, aData.projectileDamage, aData.ProjectileCount, aData.firingArc, aData.distanceFromCaster, aData.critChance);
        _statusEffects = aData.effects;
        _projectileAttack = aData.projectilePrefab;
        upgradeHandler = new ProjectileUpgradeHandler(this);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        upgradeHandler.ApplyUpgrade(upgradeEffect);
    }

    public override void CastAbility()
    {
        if (castTime > cooldown) { AdjustCooldown(castTime); }

        for (int i = 0; i < ProjData.projectileCount; i++)
        {
            // Ignore firing arc for singular projectiles 
            if (ProjData.projectileCount == 1)
            {
                ProjData.firingArc = 0;
            }
            else
            {
                SetFiringRotation(ProjData.firingArc, AbilityUtils.GetFiringArcIncrement(i, ProjData.firingArc, ProjData.projectileCount));
            }

            GameObject projectile = Object.Instantiate(_projectileAttack.gameObject, AbilityUtils.GetClosestPointToMouse(_caster.transform.position, _caster.ProjectileSpawnPos.position, ProjData.distanceFromCaster), _caster.ProjectileSpawnPos.rotation);
            projectile.GetComponent<ProjectileAttack>().Initialize(this);
        }

        // Currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(_caster.transform.position), castTime);
        EventManager.Raise(setDirectionEvent);
    }

    void SetFiringRotation(float arc, float increment)
    {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _caster.transform.position;
        float lookAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - (arc / 2);

        _caster.ProjectileSpawnPos.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f + increment);
    }

    public void OnHit(Collider2D collision, ProjectileAttack attack) 
    {
        IHealth hitHealth = collision.GetComponentInParent<IHealth>();

        float randomNumber = Random.Range(0, 1f);

        if (randomNumber < ProjData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            hitHealth.ChangeHealth((ProjData.projectileDamage * 2), true);
        }
        else 
        {

            hitHealth.ChangeHealth(ProjData.projectileDamage);
        }

        if(_statusEffects.Count> 0) 
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in _statusEffects) 
            {
                effect.ApplyEffect(statusController, _caster);
            }
        }
    }
}
