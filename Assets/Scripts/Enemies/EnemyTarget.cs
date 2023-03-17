using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyTarget : AIDestinationSetter
{
    void Awake()
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
