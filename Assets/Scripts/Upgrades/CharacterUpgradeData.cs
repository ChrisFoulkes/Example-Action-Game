using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterUpgradeEffect : UpgradeEffect
{
    public CharacterUpgradeTypes upgradeType;

    public StatData statData = null;
}

public enum CharacterUpgradeTypes 
{
    speed,
    heal, 
    stat
}
[CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "ScriptableObjects/CharacterUpgrade")]
public class CharacterUpgradeData : UpgradeData
{
    public new List<CharacterUpgradeEffect> upgradeEffects;
}