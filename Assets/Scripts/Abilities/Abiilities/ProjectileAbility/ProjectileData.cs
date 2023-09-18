using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, CasterContext caster)
    {
        return new ProjectileAbility((ProjectileData)abilityData, caster);
    }
}

[CreateAssetMenu(fileName = "Projectile", menuName = "Abilities/ProjectileData")]
public class ProjectileData : HitAbilityData
{
    [Header("Projectile Base Data")]
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    public StatAssociation critChance;
    public float distanceFromCaster = -0.2f;

    public int ProjectileCount;


    public float firingArc; [Header("Ability Prefab")]
    public ProjectileAttack projectilePrefab;

    [Header("On Hit Effects")]
    public List<StatusEffect> statusEffects;
    [InlineEditor(InlineEditorModes.FullEditor)]
    public List<BuffData> buffEffects;
    public override IAbilityFactory AbilityFactory => new ProjectileAbilityFactory();

}
