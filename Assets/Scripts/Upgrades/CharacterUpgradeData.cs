using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterUpgradeTypes 
{
    speed,
    heal,
    max_hp
}
[CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "ScriptableObjects/CharacterUpgrade")]
public class CharacterUpgradeData : UpgradeData
{
    public CharacterUpgradeTypes upgradeType;

    public float amount;
}

