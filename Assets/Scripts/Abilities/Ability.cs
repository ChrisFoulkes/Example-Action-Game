using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public UpgradeHandler upgradeHandler;

    //cooldown --
    private bool IsAvailable = true;
    public float cooldown { get; private set; }
    
    public Sprite sprite { get; private set; }
    public int ID { get; private set; }

    public AbilityType abilityType { get; private set; }

    public void adjustCooldowm(float adjustedValue) 
    {
        cooldown = adjustedValue;

    }
    public abstract void ApplyUpgrade(UpgradeEffect upgradeEffect);

    public void SetCoolDown(bool isOnCooldown) 
    {
        IsAvailable = !isOnCooldown;
    }
    public Ability(AbilityData aData) 
    {
        cooldown = aData.cooldown;
        abilityType = aData.abilityType;
        sprite = aData.abilityIcon;
        ID = aData.AbilityID;

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
