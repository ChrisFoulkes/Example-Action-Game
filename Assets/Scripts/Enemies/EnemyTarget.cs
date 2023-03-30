using Pathfinding;
using UnityEngine;

public class EnemyTarget : AIDestinationSetter
{
    private new void Awake()
    {
        GameObject player = GameObject.Find("EnemyTargetPosition");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }
    }


}
