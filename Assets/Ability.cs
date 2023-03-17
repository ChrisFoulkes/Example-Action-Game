using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    //cooldown --
    private bool IsAvailable = true;
    public float cooldown { get; private set; }

    public void SetCoolDown(bool isOnCooldown) 
    {
        IsAvailable = !isOnCooldown;
    }
    public Ability(AbilityData aData) 
    {
        cooldown = aData.cooldown;
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
