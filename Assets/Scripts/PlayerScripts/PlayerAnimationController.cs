using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update

    private Vector2 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
        PlayerDeathEvent.RegisterListener(OnPlayerDeath);
        SetPlayerFacingDirectionEvent.RegisterListener(SetPlayerRotation);
        PlayerStopMovementEvent.RegisterListener(OnStopMovementEvent);

    }

    void OnDestroy()
    {
        PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
        SetPlayerFacingDirectionEvent.UnregisterListener(SetPlayerRotation);
        PlayerStopMovementEvent.UnregisterListener(OnStopMovementEvent);
    }

    private void Update()
    {
        // Calculate the object's movement direction
        Vector2 movement = (Vector2)transform.position - lastPosition;

        // Set the animator parameters based on the movement direction
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", 0f);
        }
        else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
        {
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Horizontal", 0f);
        }

        // Update the last position
        lastPosition = transform.position;
    }

    void SetPlayerRotation(SetPlayerFacingDirectionEvent directionEvent)
    {
        if (directionEvent.direction == Vector2.left)
        {
            animator.SetTrigger("forceFaceLeft");
        }
        if (directionEvent.direction == Vector2.right)
        {

            animator.SetTrigger("forceFaceRight");
        }
        if (directionEvent.direction == Vector2.up)
        {

            animator.SetTrigger("forceFaceUp");
        }
        if (directionEvent.direction == Vector2.down)
        {

            animator.SetTrigger("forceFaceDown");
        }
    }

    private void StopMovement()
    {
        animator.SetFloat("Vertical", 0f);
        animator.SetFloat("Horizontal", 0f);
    }

    void OnStopMovementEvent(PlayerStopMovementEvent stop)
    {
        StopMovement();
    }

    void OnPlayerDeath(PlayerDeathEvent death)
    {
        StopMovement();
        animator.SetTrigger("IsDead");
    }
}
