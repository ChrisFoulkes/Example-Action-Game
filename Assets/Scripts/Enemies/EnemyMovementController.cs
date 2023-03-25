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
    private Rigidbody2D Rb2D;

    public bool isKnockbackAble = true;
    private float baseMovementSpeed = 3f;
    // Start is called before the first frame update
    void Awake()
    {
        aiPath= GetComponentInParent<AIPath>();
        Rb2D= GetComponentInParent<Rigidbody2D>();


    }


    public void Initialize(float initialMovement)
    {
        baseMovementSpeed = initialMovement; 
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

    public void HandleKnockback(Vector2 knockbackDirection, Vector2 knockbackForce, float delay) 
    {
        if (isKnockbackAble)
        {
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce, delay));
        }
    
    }
    public IEnumerator ApplyKnockback(Vector2 knockbackDirection, Vector2 knockbackForce, float delay)
    {
        aiPath.canMove = false;
        CanMove(false);


        if (Rb2D != null)
        {
            Rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(delay);
        CanMove(true);
        aiPath.canMove = true;
    }
}
