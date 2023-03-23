using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatData : ScriptableObject
{
    public int ID;
    public string Name;
    public float baseValue;

    public StatData(int id, string name, float value)
    {
        ID = id;
        Name = name;
        baseValue = value;
    }
}


[CreateAssetMenu(fileName = "AdditivestatData", menuName = "Stats/AdditiveStat", order = 2)]
public class AdditiveStatData : StatData
{
    public AdditiveStatData(int id, string name, float value) : base(id, name, value) { }
}

