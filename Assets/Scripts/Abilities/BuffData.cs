using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AffectStat
{
    public StatData stat;
    public float amount;    
}


[CreateAssetMenu(fileName = "BuffData", menuName = "Abilities/Buff")]
public class BuffData : AbilityData
{
    [Header("Buff Base Data")]
    public int BuffID;
    public List<AffectStat> affectStats = new List<AffectStat>();
    public float baseBuffDuration;
}
