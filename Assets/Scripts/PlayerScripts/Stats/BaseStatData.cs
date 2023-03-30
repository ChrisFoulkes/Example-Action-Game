using UnityEngine;


[CreateAssetMenu(fileName = "BaseStatData", menuName = "Stats/BaseStat", order = 1)]
public class BaseStatData : StatData
{
    public BaseStatData(int id, string name, float value) : base(id, name, value) { }
}