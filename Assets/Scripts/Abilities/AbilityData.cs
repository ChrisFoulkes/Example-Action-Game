using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType 
{
    projectile,
    melee,
    buff,
    movement
}
public class AbilityData : ScriptableObject
{
    [Header("Generic Ability")]
    public int AbilityID;
    public string abilityName;
    public Sprite abilityIcon;
    public AbilityType abilityType;
    public float cooldown = 0.5f;
    public float castTime = 0.1f;
}
