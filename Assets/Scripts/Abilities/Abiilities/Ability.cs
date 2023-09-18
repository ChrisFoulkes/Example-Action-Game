using UnityEngine;

public abstract class HitAbility : Ability
{
    public float hitStun;
    public KnockbackData knockbackData;

    public HitAbility(HitAbilityData aData) : base(aData)
    {
        hitStun = aData.hitStun;
        knockbackData= aData.knockbackData;
    }


    public virtual bool IsCriticalHit(float critChance) // needs work should calc the crit in this function 
    {
        float randomNumber = Random.Range(0f, 1f);

        if (randomNumber < critChance)
        {
            return true;
        }

        return false;
    }

    public abstract void OnHit(TargetContext targetContext, Attack attack);
}

public abstract class Ability
{
    public UpgradeHandler AbilityUpgradeHandler;

    // Cooldown properties
    private bool IsAvailable { get; set; } = true;


    public float Cooldown{get;  private set;}
    public float CooldownEndTime { get; private set; }
    public float CastTime;

    // Ability properties
    public Sprite Sprite { get; private set; }
    public int ID { get; private set; }
    public AbilityType AbilityType { get; private set; }
    public bool IsBufferable { get; private set; }
    public bool ShouldStopMovementOnCast { get; private set; }
    public bool ShouldForceCastingDirection { get; private set; }

    public void AdjustCooldown(float adjustedValue)
    {
        Cooldown = adjustedValue;
        if (CastTime > Cooldown) 
        {
            Debug.Log("Adjusting Ability: " + ID + " Cooldown below cast time");
            Cooldown = CastTime; }

    }
    public virtual void ApplyUpgrade(UpgradeEffect upgradeEffect) 
    {
        AbilityUpgradeHandler.ApplyUpgrade(upgradeEffect);
    }

    public void SetCoolDown(bool isOnCooldown)
    {
        IsAvailable = !isOnCooldown;
        CooldownEndTime = Time.time + Cooldown;
    }
    public float RemainingCooldown()
    {
        if (!IsAvailable)
        {
            return CooldownEndTime - Time.time;
        }
        return 0;
    }

    public Ability(AbilityData aData)
    {
        Cooldown = aData.cooldown;
        AbilityType = aData.abilityType;
        Sprite = aData.abilityIcon;
        ID = aData.AbilityID;
        CastTime = aData.castTime;
        IsBufferable = aData.isBufferable;
        ShouldStopMovementOnCast = aData.shouldStopMovementOnCast;
        ShouldForceCastingDirection= aData.shouldForceCastingDirection;
    }

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

    public void CastAbility() 
    {
        OnCast();
    }

    protected abstract void OnCast();
}


//Stages of an attack 

//On cast (when you click the button and the ability resolves  .Implemented

//On hit (when you hit an enemy)  .Implemented

//On damage (when you damage an enemy) ? not sure if needed 

//On miss (when you hit nothing) awkward to do 

//On complete (when the attack is completely resolved) .Not implemented


