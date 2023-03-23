using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public enum StatType 
{
    baseValue,
    additive,
    multiplicative
}
public class ActiveStat
{
    public StatType type;
    public int ID;
    public string Name;
    public float value;

    public ActiveStat(int id, string name, float value, StatType type)
    {
        ID = id;
        Name = name;
        this.value = value;
        this.type = type;
    }
}
public class CharacterStatsController : MonoBehaviour
{
    [SerializeField] private List<StatData> Stats;

    public Dictionary<int, ActiveStat> activeStats = new Dictionary<int, ActiveStat>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateActiveStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateActiveStats()
    {
        foreach (StatData stat in Stats)
        {
            StatType newStatType;
            if (stat is BaseStatData)
            {
                newStatType = StatType.baseValue;
            }
            else if (stat is AdditiveStatData)
            {
                newStatType = StatType.additive;
            }
            else
            {
                newStatType = StatType.multiplicative;
            }

            ActiveStat newStat = new ActiveStat(stat.ID, stat.name, stat.baseValue, newStatType);

            activeStats.Add(newStat.ID, newStat);
        }
    }

    public void AlterStat(int Id, float amount)
    {
        ActiveStat affectedStat = null;
        if (!activeStats.ContainsKey(Id))
        {
            Debug.LogWarning("Upgrade Failed missing : " +  "Stat.id " + Id);
        }
        else
        {
            affectedStat = activeStats[Id];
        }

        if (affectedStat != null)
        {
            affectedStat.value += amount;
        }
    }
}
