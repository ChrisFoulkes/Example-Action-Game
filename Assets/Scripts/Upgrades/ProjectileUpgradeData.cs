using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProjectileUpgradeEffect : UpgradeEffect
{
    public ProjectileUpgradeTypes upgradeType;
}


public enum ProjectileUpgradeTypes
{
    projectileCount, 
    projectileArc,
    projectileDamage
}


[CreateAssetMenu(fileName = "ProjectileUpgrade", menuName = "ScriptableObjects/ProjectileUpgrades")]
public class ProjectileUpgradeData : UpgradeData
{
    public ProjectileData ability;
    public new List<ProjectileUpgradeEffect> upgradeEffects;
}




