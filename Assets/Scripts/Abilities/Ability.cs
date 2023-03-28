using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public UpgradeHandler upgradeHandler;

    //cooldown --
    private bool IsAvailable = true;
    public float cooldown { get; private set; }
    public float cooldownEndTime;
    public float castTime = 0.1f; 

    public Sprite sprite { get; private set; }
    public int ID { get; private set; }

    public AbilityType abilityType { get; private set; }

    public bool isBufferable = false;

    public void AdjustCooldown(float adjustedValue) 
    {
        cooldown = adjustedValue;

    }
    public abstract void ApplyUpgrade(UpgradeEffect upgradeEffect);

    public void SetCoolDown(bool isOnCooldown) 
    {
        IsAvailable = !isOnCooldown;
        cooldownEndTime = Time.time + cooldown;
    }
    public float RemainingCooldown()
    {
        if (!IsAvailable)
        {
            return cooldownEndTime - Time.time;
        }
        return 0;
    }

    public Ability(AbilityData aData) 
    {
        cooldown = aData.cooldown;
        abilityType = aData.abilityType;
        sprite = aData.abilityIcon;
        ID = aData.AbilityID;
        castTime = aData.castTime;
        isBufferable = aData.isBufferable;


}
    public abstract void CastAbility();


    public bool IsCastable() 
    {
        if (IsAvailable) 
        {
            return true;
        }
        else 
        {
            return false;
        } 
    }

}
