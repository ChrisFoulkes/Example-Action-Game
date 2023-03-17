using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeTier 
{
    normal, 
    rare, 
    epic,
    legendary
}

public enum BaseUpgradeType
{ 
    characterUpgrade, 
    attackUpgrade
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeIcon;
    public BaseUpgradeType baseUpgradeType;

    public UpgradeTier tier;
}
