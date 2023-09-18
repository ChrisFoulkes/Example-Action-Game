using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


public class MouseTargetAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, CasterContext caster)
    {
        return new MouseTargetAbility((MouseTargetData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "MouseTargetData", menuName = "Abilities/MouseTargetData")]
public class MouseTargetData : HitAbilityData
{
    [Header("Projectile Base Data")]
    public StatAssociation Damage;
    public StatAssociation critChance;


    [Header("Ability Prefab")]
    public GameObject attackPrefab;


    [Header("On Hit Effects")]
    public List<StatusEffect> statusEffects;
    [InlineEditor(InlineEditorModes.FullEditor)]
    public List<BuffData> buffEffects;

    public override IAbilityFactory AbilityFactory => new MouseTargetAbilityFactory();
}