using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class MovementController: MonoBehaviour, IMovement
{
    float normalMaxSpeed = 5;
    float currentMaxSpeed = 5;

    private Pathfinding.AIPath aiPath;

    public void Start()
    {
        aiPath = GetComponent<Pathfinding.AIPath>();
        aiPath.maxSpeed = currentMaxSpeed;
    }

    public void CanMove(bool canMove)
    {
        aiPath.canMove = canMove;

        if (!canMove) 
        {
            aiPath.OnTargetReached();
        }
    }
    public float CurrentMaxSpeed()
    {
        return currentMaxSpeed;
    }

    public void SetMovementSpeed(float speed)
    { 
        currentMaxSpeed = speed;
    }
    public void ChangeSpeed(float amount) 
    {
        //need to alter the stopping distance + end point reached to match increasing speeds 
        //currently cap at 15)
        if (currentMaxSpeed < 15)
        {
            currentMaxSpeed += amount;
            if (currentMaxSpeed > 15) { currentMaxSpeed = 15; }
            aiPath.maxSpeed = currentMaxSpeed;
        }
    }
}
