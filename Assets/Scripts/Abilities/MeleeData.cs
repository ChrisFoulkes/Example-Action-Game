using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityCasterContext caster)
    {
        return new MeleeAbility((MeleeData)abilityData, caster);
    }
}


[CreateAssetMenu(fileName = "MeleeData", menuName = "Abilities/Melee")]
public class MeleeData : HitAbilityData
{
    [Header("Melee Base Data")]
    public StatAssociation meleeDamage;
    public StatAssociation critChance;

    public float distanceFromCaster = -0.2f;

    public MeleeAttack meleePrefab;

    public List<StatusEffect> effects;
    public override IAbilityFactory AbilityFactory => new MeleeAbilityFactory();
}
