using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeEffect 
{
    public ProjectileUpgradeTypes upgradeType;
    public float amount;
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
    public List<UpgradeEffect> upgradeEffects;
}


