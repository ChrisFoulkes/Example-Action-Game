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
    [field: SerializeField] public int BuffID { get; private set; }
    [field: SerializeField] public List<AffectedStat> AffectedStats { get; private set; } = new List<AffectedStat>();
    [field: SerializeField] public float BaseBuffDuration { get; private set; }
    public override IAbilityFactory AbilityFactory => new BuffAbilityFactory();
}