using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterUpgradeEffect : UpgradeEffect
{
    public CharacterUpgradeTypes upgradeType;
}

public enum CharacterUpgradeTypes 
{
    speed,
    heal
}
[CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "ScriptableObjects/CharacterUpgrade")]
public class CharacterUpgradeData : UpgradeData
{
    public new List<CharacterUpgradeEffect> upgradeEffects;
}