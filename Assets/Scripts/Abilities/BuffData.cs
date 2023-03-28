using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AffectStat
{
    public StatData stat;
    public float amount;    
}

public class BuffAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityContext caster)
    {
        return new BuffAbility((BuffData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "BuffData", menuName = "Abilities/Buff")]
public class BuffData : AbilityData
{
    [Header("Buff Base Data")]
    public int BuffID;
    public List<AffectStat> affectStats = new List<AffectStat>();
    public float baseBuffDuration;
    public override IAbilityFactory AbilityFactory => new BuffAbilityFactory();
}
