using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiplicativestatData", menuName = "Stats/MultiplicativeStat", order = 3)]
public class MultiplicativeStatData : StatData
{
    public MultiplicativeStatData(int id, string name, float value) : base(id, name, value) { }
}