using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EventCallbacks;

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

public class ProjectileAbility : HitAbility
{
    private PlayerCasterContext _caster;
    public ActiveProjectileData ProjData { get; }
    private readonly ProjectileAttack _projectileAttack;
    private List<StatusEffect> _statusEffects;
    private List<ActiveBuffData> _buffEffects = new List<ActiveBuffData>();

    public ProjectileAbility(ProjectileData aData, CasterContext caster) : base(aData)
    {
        _caster = (PlayerCasterContext)caster;
        ProjData = new ActiveProjectileData(aData.projectileSpeed, aData.projectileLifetime, aData.projectileDamage, aData.ProjectileCount, aData.firingArc, aData.distanceFromCaster, aData.critChance);
        _statusEffects = aData.statusEffects;

        foreach (BuffData data in aData.buffEffects)
        {
            _buffEffects.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID, data.isStackable, data.maxStacks));
        }

        _projectileAttack = aData.projectilePrefab;
        AbilityUpgradeHandler = new ProjectileUpgradeHandler(this);
    }


    protected override void OnCast()
    {
        for (int i = 0; i < ProjData.projectileCount; i++)
        {
            SetFiringRotation(ProjData.firingArc, AbilityUtils.GetFiringArcIncrement(i, ProjData.firingArc, ProjData.projectileCount));
            GameObject projectile = Object.Instantiate(_projectileAttack.gameObject, AbilityUtils.GetClosestPointToMouse(_caster.transform.position, _caster.AttackSpawnPos.position, ProjData.distanceFromCaster), _caster.AttackSpawnPos.rotation);
            projectile.GetComponent<ProjectileAttack>().Initialize(this);
        }
    }

    void SetFiringRotation(float arc, float increment)
    {
        if (ProjData.projectileCount == 1)
        {
            arc = 0;
        }

        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _caster.transform.position;
        float lookAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - (arc / 2);

  
        _caster.AttackSpawnPos.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f + increment);
    }

    public override void OnHit(TargetContext target, Attack attack)
    {

        float critRoll = Random.Range(0, 1f);
        int finalDamage = ProjData.projectileDamage;
        bool isCrit = false;

        if (critRoll < ProjData.critChance.CalculateModifiedValue(_caster.CharacterStatsController))
        {
            finalDamage *= 2;
            isCrit = true;
        }

        DamageInfo damageInfo = new DamageInfo(finalDamage, isCrit, FloatingColourType.Generic, hitStun, knockbackData);

        target.DamageController.ApplyDamage(damageInfo, _caster);


        //Potentially damage event should include status effects applied etc.

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
