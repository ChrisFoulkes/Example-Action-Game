using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatAssociation
{
    public string propertyName;
    public float baseValue = 0;
    public List<StatData> associatedStats;


    public StatAssociation(StatAssociation stat)
    {
        this.propertyName = stat.propertyName;
        this.baseValue = stat.baseValue;
        this.associatedStats = stat.associatedStats;
    }

    public float CalculateModifiedValue(CharacterStatsController caster)
    {
        float modifiedValue = baseValue;
        float additiveValue = 0f;
        float multiplicativeValue = 1f;

        if (associatedStats != null)
        {
            foreach (StatData stat in associatedStats)
            {
                ActiveStat casterStat = null;
                if (!caster.activeStats.ContainsKey(stat.ID))
                {
                    Debug.LogWarning("Caster missing " + stat.name + ": " + stat.ID);
                }
                else 
                {
                    casterStat = caster.activeStats[stat.ID];
                }

                if (casterStat != null)
                {
                    if (casterStat.type == StatType.baseValue)
                    {
                        modifiedValue += casterStat.value;
                    }
                    else if (casterStat.type == StatType.additive)
                    {
                        additiveValue += casterStat.value;
                    }
                    else if (casterStat.type == StatType.multiplicative)
                    {
                        multiplicativeValue *= 1f + casterStat.value;
                    }
                }
            }
        }

        modifiedValue += modifiedValue * additiveValue;
        modifiedValue *= multiplicativeValue;

        return modifiedValue;
    }
}