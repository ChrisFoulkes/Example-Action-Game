using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, CasterContext caster)
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

    [Header("On Hit Effects")]
    public List<StatusEffect> statusEffects;
    [InlineEditor(InlineEditorModes.FullEditor)]
    public List<BuffData> buffEffects;

    [Header("Effect Asset")]
    public MeleeAttack meleePrefab;
    public override IAbilityFactory AbilityFactory => new MeleeAbilityFactory();
}
