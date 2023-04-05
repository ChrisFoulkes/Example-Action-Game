using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AffectedStat
{
    public StatData Stat;
    public float Amount;
}

public class BuffAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityCasterContext caster)
    {
        return new BuffAbility((BuffAbilityData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "BuffData", menuName = "Abilities/Buff")]
public class BuffAbilityData : AbilityData
{

    public List<BuffData> BuffData = new List<BuffData>();
    public override IAbilityFactory AbilityFactory => new BuffAbilityFactory();
}