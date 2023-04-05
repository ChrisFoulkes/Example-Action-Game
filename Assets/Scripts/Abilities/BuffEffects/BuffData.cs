using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/buffData")]

public class BuffData : ScriptableObject
{
    public int BuffID;
    public List<AffectedStat> AffectedStats;
    public float BuffDuration;
}
