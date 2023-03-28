using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombustionAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityContext caster)
    {
        return new CombustionAbility((CombustionData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "CombustionData", menuName = "Abilities/Special/Combustion")]
public class CombustionData : AbilityData
{
    [Header("Combustion Data")]
    public StatAssociation combustionDamage;
    public GameObject effectObject;
    public override IAbilityFactory AbilityFactory => new CombustionAbilityFactory();
}
