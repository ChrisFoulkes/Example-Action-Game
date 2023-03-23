using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEffect
{
    public float amount;
}

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
    abilityUpgrade
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [Header("No. of times you can choose this upgrade")]
    public int upgradeLimit = -1;
    [Header("Upgrade Info")]
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeIcon;
    public BaseUpgradeType baseUpgradeType;
    public UpgradeTier tier;
    public List<UpgradeEffect> upgradeEffects = new List<UpgradeEffect>();



}
