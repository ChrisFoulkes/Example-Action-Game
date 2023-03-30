using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityFactory : IAbilityFactory
{
    public Ability Create(AbilityData abilityData, AbilityCasterContext caster)
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

    public List<StatusEffect> effects;
    public override IAbilityFactory AbilityFactory => new ProjectileAbilityFactory();

}
