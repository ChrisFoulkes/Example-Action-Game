using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityContext caster)
    {
        return new MovementAbility((MovementData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "MovementData", menuName = "Abilities/Movement", order = 3)]
public class MovementData : AbilityData
{
    public float movementSpeed;
    public float movementDuration;
    public override IAbilityFactory AbilityFactory => new MovementAbilityFactory();
}
