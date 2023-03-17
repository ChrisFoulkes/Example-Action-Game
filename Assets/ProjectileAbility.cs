using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    //projectile activity --
    private float projectileSpeed;
    private float projectileLifetime;
    private int projectileDamage;
    //projectile firing -- 
    private int ProjectileCount;
    private float firingArc;
    private float castTime;

    private ProjectileAttack projectileAttack;
    private Transform caster;
    private Transform projectileSpawnPoint;

    public ProjectileAbility(ProjectileData aData, Transform casterTransform, Transform projectileSpawnPos): base(aData)
    {
        projectileSpeed = aData.projectileSpeed;
        projectileLifetime = aData.projectileLifetime;
        projectileDamage= aData.projectileDamage;

        ProjectileCount = aData.ProjectileCount;
        firingArc= aData.firingArc;
        castTime = aData.castTime;
        projectileAttack = aData.projectilePrefab;

        caster = casterTransform;
        projectileSpawnPoint = projectileSpawnPos;
    }


     public override void CastAbility()
    {

        for (int i = 0; i < ProjectileCount; i++)
        {

            //ignore firing arc for singular projectiles 
            if (ProjectileCount == 1)
            {
                firingArc = 0;
            }

            SetTheFiringRotation(firingArc, getIncrement(i, firingArc, ProjectileCount));
            GameObject projectile = Object.Instantiate(projectileAttack.gameObject, GetClosestPoint(), projectileSpawnPoint.rotation);

            //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }

        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        stopMovementEvent.FireEvent();

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(getFacingDirection(), castTime);
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

    Vector3 GetClosestPoint()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - caster.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        Vector3 closestPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * 0.5f;
        closestPoint += projectileSpawnPoint.position;

        return closestPoint;
    }
}
