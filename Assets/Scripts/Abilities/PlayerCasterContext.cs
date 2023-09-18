using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasterContext : CasterContext
{
    public Transform AttackSpawnPos { get; }
    public PlayerMovement PlayerMovement { get; }
    public CharacterStatsController CharacterStatsController { get; }
    public PlayerStatusTracker PlayerStatusTracker { get; }
    public BuffController BuffController { get; }

    public PlayerCasterContext(Transform caster, Transform attackSpawnPos) : base(caster)
    {
        AttackSpawnPos = attackSpawnPos;
        PlayerMovement = caster.GetComponent<PlayerMovement>();
        CharacterStatsController = caster.GetComponent<CharacterStatsController>();
        BuffController = caster.GetComponent<BuffController>();
        PlayerStatusTracker = caster.GetComponentInChildren<PlayerStatusTracker>();

    }
}

