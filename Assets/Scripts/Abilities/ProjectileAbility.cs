using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveProjectileData 
{
    //projectile activity --
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    //projectile firing -- 
    public int projectileCount;
    public float firingArc;
    public float castTime;

    public ActiveProjectileData(float projectileSpeed, float projectileLifetime, int projectileDamage, int projectileCount, float firingArc, float castTime)
    {
        this.projectileSpeed = projectileSpeed;
        this.projectileLifetime = projectileLifetime;
        this.projectileDamage = projectileDamage;
        this.projectileCount = projectileCount;
        this.firingArc = firingArc;
        this.castTime = castTime;
    }

}
public class ProjectileAbility : Ability
{
    public  ActiveProjectileData projData;

    private ProjectileAttack projectileAttack;
    private Transform caster;
    private Transform projectileSpawnPoint;

    //testing -- version 
    private List<StatusEffect> statusEffects;

    public ProjectileAbility(ProjectileData aData, Transform casterTransform, Transform projectileSpawnPos): base(aData)
    {
        projData = new ActiveProjectileData(aData.projectileSpeed, aData.projectileLifetime, aData.projectileDamage, aData.ProjectileCount, aData.firingArc, aData.castTime);

        statusEffects = aData.effects;
        projectileAttack = aData.projectilePrefab;

        caster = casterTransform;
        projectileSpawnPoint = projectileSpawnPos;
    }


     public override void CastAbility()
    {

        for (int i = 0; i < projData.projectileCount; i++)
        {

            //ignore firing arc for singular projectiles 
            if (projData.projectileCount == 1)
            {
                projData.firingArc = 0;
            }

            SetTheFiringRotation(projData.firingArc, AbilityUtils.getFiringArcIncrement(i, projData.firingArc, projData.projectileCount));
            GameObject projectile = Object.Instantiate(projectileAttack.gameObject, AbilityUtils.GetClosestPointToMouse(caster.position, projectileSpawnPoint.position), projectileSpawnPoint.rotation);
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
        hitHealth.ChangeHealth(projData.projectileDamage);

        if(statusEffects.Count> 0) 
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in statusEffects) 
            {
                effect.ApplyEffect(statusController);
            }
        }
    }
}
