using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseTargetAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityContext caster)
    {
        return new MouseTargetAbility((MouseTargetData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "MouseTargetData", menuName = "Abilities/MouseTargetData")]
public class MouseTargetData : AbilityData
{
    [Header("Projectile Base Data")]
    public int projectileDamage;
    public StatAssociation critChance;

  
    [Header("Ability Prefab")]
    public GameObject attackPrefab;
    public List<StatusEffect> effects;
    public override IAbilityFactory AbilityFactory => new MouseTargetAbilityFactory();
}