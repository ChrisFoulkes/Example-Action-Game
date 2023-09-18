using UnityEngine;

public enum AbilityType
{
    projectile,
    melee,
    buff,
    movement,
    targetless,
    mouseTargeted
}
public interface IAbilityFactory
{
    Ability Create(AbilityData abilityData, CasterContext abilityContext);
}

public abstract class HitAbilityData : AbilityData
{
    [Header("Knockback Data")]
    public KnockbackData knockbackData;
    public float hitStun;
}

public abstract class AbilityData : ScriptableObject
{
    [Header("Generic Ability")]
    public int AbilityID;
    public string abilityName;
    public Sprite abilityIcon;
    public AbilityType abilityType;
    public float cooldown = 0.5f;
    public float castTime = 0.1f;
    public bool isBufferable = false;
    public bool shouldStopMovementOnCast = true;
    public bool shouldForceCastingDirection = true;

    public abstract IAbilityFactory AbilityFactory { get; }
}
