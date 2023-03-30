using System.Collections.Generic;

public class ActiveBuffData
{
    public int BuffID { get; private set; }
    public List<AffectedStat> AffectedStats { get; private set; }
    public float BuffDuration { get; private set; }

    public ActiveBuffData(List<AffectedStat> affectStats, float duration, int id)
    {
        BuffID = id;
        BuffDuration = duration;
        AffectedStats = affectStats;
    }
}