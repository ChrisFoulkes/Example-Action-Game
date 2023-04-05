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

    public void AdjustCooldown(float adjustedValue)
    {
        Cooldown = adjustedValue;
        if (CastTime > Cooldown) 
        {
            Debug.Log("Adjusting Ability: " + ID + " Cooldown below cast time");
            Cooldown = CastTime; }

    }
    public abstract void ApplyUpgrade(UpgradeEffect upgradeEffect);

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

    public bool IsCriticalHit(float critChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (randomNumber < critChance)
        {
            return true;
        }
        
        return false;
    }
    public abstract void CastAbility();


}
