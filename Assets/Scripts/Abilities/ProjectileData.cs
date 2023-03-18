using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Abilities/ProjectileData")]
public class ProjectileData : AbilityData
{
    [Header("Projectile Data")]
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;
    public float castTime;

    public int ProjectileCount;


    public float firingArc; [Header("Ability Prefab")]
    public ProjectileAttack projectilePrefab;
}