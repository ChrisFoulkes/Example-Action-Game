using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MovementData", menuName = "Abilities/Movement", order = 3)]
public class MovementData : AbilityData
{
    public float movementSpeed;
    public float movementDuration;
}
