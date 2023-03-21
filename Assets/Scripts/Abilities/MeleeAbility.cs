using EventCallbacks;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ActiveMeleeData
{
    public int meleeDamage;
    public float originalCastTime;
    public float castTime;

    public ActiveMeleeData(int meleeDamage, float castTime)
    {
        this.meleeDamage = meleeDamage;
        this.castTime = castTime;

        this.originalCastTime = castTime;
    }

}
public class MeleeAbility : Ability
{
    public ActiveMeleeData meleeData;

    private MeleeAttack meleeAttack;
    private Transform caster;
    private Transform projectileSpawnPoint;


    private List<StatusEffect> statusEffects;

    public MeleeAbility(MeleeData aData, Transform casterTransform, Transform projectileSpawnPos) : base(aData)
    {
        meleeData = new ActiveMeleeData(aData.meleeDamage, aData.castTime);

        statusEffects = aData.effects;
        meleeAttack = aData.meleePrefab;

        caster = casterTransform;
        projectileSpawnPoint = projectileSpawnPos;
    }


    public override void CastAbility()
    {
        if (meleeData.castTime > cooldown){ adjustCooldowm(meleeData.castTime); }

        GameObject melee = Object.Instantiate(meleeAttack.gameObject, projectileSpawnPoint.position, SetTheFiringRotation());

        melee.GetComponent<MeleeAttack>().Initialize(this);


        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = meleeData.castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.getFacingDirection(caster.position), meleeData.castTime);
        EventManager.Raise(setDirectionEvent);
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

        float randomNumber = UnityEngine.Random.Range(0, 100);

        if (randomNumber < 10)
        {
            hitHealth.ChangeHealth((meleeData.meleeDamage * 2), true);
        }
        else
        {

            hitHealth.ChangeHealth(meleeData.meleeDamage);
        }

        if (statusEffects.Count > 0)
        {
            StatusEffectController statusController = collision.GetComponentInParent<StatusEffectController>();

            foreach (var effect in statusEffects)
            {
                effect.ApplyEffect(statusController);
            }
        }
    }
}
