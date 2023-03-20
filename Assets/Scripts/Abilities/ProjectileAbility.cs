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

    public ProjectileAbility(ProjectileData aData, Transform casterTransform, Transform projectileSpawnPos): base(aData)
    {
        projData = new ActiveProjectileData(aData.projectileSpeed, aData.projectileLifetime, aData.projectileDamage, aData.ProjectileCount, aData.firingArc, aData.castTime);

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

            SetTheFiringRotation(projData.firingArc, getIncrement(i, projData.firingArc, projData.projectileCount));
            GameObject projectile = Object.Instantiate(projectileAttack.gameObject, GetClosestPointToMouse(caster.position, projectileSpawnPoint.position), projectileSpawnPoint.rotation);
            projectile.GetComponent<ProjectileAttack>().Initialize(projData.projectileDamage, projData.projectileSpeed, projData.projectileLifetime);
        }

        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = projData.castTime;
        stopMovementEvent.FireEvent();

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(getFacingDirection(), projData.castTime);
        setDirectionEvent.FireEvent();
    }



    float getIncrement(float index, float arc, int count)
    {
        if (count == 1)
            return 0;

        return index * arc / (count - 1);

    }
    void SetTheFiringRotation(float arc, float increment)
    {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.position;
        float lookAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - (arc / 2);

        projectileSpawnPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f + increment);
    }

    Vector2 getFacingDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = caster.position;

        // Get the direction from the player to the mouse
        Vector2 direction = mousePosition - playerPosition;

        // Get the angle of the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the facing direction based on the angle
        Vector2 facingDirection;
        if (angle > -45f && angle <= 45f)
        {
            return facingDirection = Vector2.right; // Right
        }
        else if (angle > 45f && angle <= 135f)
        {
            return facingDirection = Vector2.up; // Up
        }
        else if (angle > 135f || angle <= -135f)
        {
            return facingDirection = Vector2.left; // Left
        }
        else
        {
            return facingDirection = Vector2.down; // Down
        }
    }

    Vector3 GetClosestPointToMouse(Vector3 CasterPosition, Vector3 spawnPoint, float distanceFromCaster = -0.2f)
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        Vector3 closestPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * distanceFromCaster;
        closestPoint += projectileSpawnPoint.position;

        return closestPoint;
    }
}
