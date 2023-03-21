using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeUpgradeEffect : UpgradeEffect
{
    public MeleeUpgradeTypes upgradeType;
}

public enum MeleeUpgradeTypes
{
    meleeDamage,
    meleeCastSpeed
}


[CreateAssetMenu(fileName = "MeleeUpgrade", menuName = "ScriptableObjects/MeleeUpgrade")]
public class MeleeUpgradeData : UpgradeData
{
    public MeleeData ability;
    public new List<MeleeUpgradeEffect> upgradeEffects;
}
