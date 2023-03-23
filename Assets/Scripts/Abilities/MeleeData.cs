using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeData", menuName = "Abilities/Melee")]
public class MeleeData : AbilityData
{
    [Header("Melee Base Data")]
    public StatAssociation meleeDamage;
    public StatAssociation critChance;
    public float castTime;
    public float distanceFromCaster = -0.2f;

    public MeleeAttack meleePrefab;

    public List<StatusEffect> effects;
}
