using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterUpgradeTypes 
{
    speed,
    heal
}
[CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "ScriptableObjects/CharacterUpgrade")]
public class CharacterUpgradeData : UpgradeData
{
    public CharacterUpgradeTypes upgradeType;

    public float amount;
}

