using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MovementAnimationState
{
    public Vector3 desiredDirection;
    public bool moving = false;

    public MovementAnimationState(Vector3 desiredVelocity, bool isMoving)
    {
        desiredDirection = desiredVelocity;
        moving = isMoving;
    }
}

public class EnemyMovementController : MonoBehaviour, IMovement
{
    AIPath aiPath;

    public float baseMovementSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        aiPath= GetComponentInParent<AIPath>();



        SetInitialMoveSpeed();
    }



    public MovementAnimationState GetMovementAnimState()
    {
        bool isMoving = (Mathf.Abs(aiPath.velocity.x) > 0 || Mathf.Abs(aiPath.velocity.y) > 0);
        return new MovementAnimationState(aiPath.desiredVelocity, isMoving);
    }

    void SetInitialMoveSpeed() 
    { 
        aiPath.maxSpeed = baseMovementSpeed;
    }

    public void CanMove(bool canMove) 
    {
        if (!canMove)
        {
            aiPath.maxSpeed = 0f;
        }
        else

        {
            aiPath.maxSpeed = baseMovementSpeed;
        }
    
    }
    public void ChangeSpeed(float amount) 
    {
        Debug.Log("Slowing Enemies not implemented!");
    }
}
