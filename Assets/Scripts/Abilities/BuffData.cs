using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Abilities/Buff")]
public class BuffData : AbilityData
{
    [Header("Buff Base Data")]
    public List<StatAssociation> affectStats = new List<StatAssociation>();    
}
