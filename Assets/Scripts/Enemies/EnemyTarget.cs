using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyTarget : AIDestinationSetter
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
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
