using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Abilities/ProjectileData")]
public class ProjectileData : AbilityData
{
    [Header("Projectile Base Data")]
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    public float castTime;
    public float distanceFromCaster = -0.2f;

    public int ProjectileCount;


    public float firingArc; [Header("Ability Prefab")]
    public ProjectileAttack projectilePrefab;

    public List<StatusEffect> effects;
}
