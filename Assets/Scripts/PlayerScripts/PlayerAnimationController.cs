using EventCallbacks;
using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    // Start is called before the first frame update

    private Vector2 lastPosition;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lastPosition = transform.position;
    }

    private void OnEnable()
    {

        EventManager.AddGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.AddGlobalListener<PlayerStopMovementEvent>(OnStopMovementEvent);
        EventManager.AddGlobalListener<SetPlayerFacingDirectionEvent>(SetPlayerRotation);
    }

    private void OnDisable()
    {

        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.RemoveGlobalListener<PlayerStopMovementEvent>(OnStopMovementEvent);
        EventManager.RemoveGlobalListener<SetPlayerFacingDirectionEvent>(SetPlayerRotation);
    }


    private void Update()
    {
        if (playerMovement.movement.x != 0 || playerMovement.movement.y != 0)
        {
            animator.SetBool("isMovementKey", true);
        }
        else
        {
            animator.SetBool("isMovementKey", false);
        }

        // Calculate the object's movement direction
        Vector2 movement = (Vector2)transform.position - lastPosition;

        // Set the animator parameters based on the movement direction
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y) * 0.75f) //*75% to force diagonals horizontal
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
            animator.SetBool("forceFaceLeft", true);
            //animator.SetTrigger("forceFaceLeft");
        }
        if (directionEvent.direction == Vector2.right)
        {

            animator.SetBool("forceFaceRight", true);
            //animator.SetTrigger("forceFaceRight");
        }
        if (directionEvent.direction == Vector2.up)
        {

            animator.SetTrigger("forceFaceUp");
        }
        if (directionEvent.direction == Vector2.down)
        {

            animator.SetTrigger("forceFaceDown");
        }

        StartCoroutine(StopForcingDirection(directionEvent.duration));
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
        animator.SetBool("Dead", true);
    }

    private IEnumerator StopForcingDirection(float stopDuration)
    {

        yield return new WaitForSeconds(stopDuration);
        {
            animator.SetBool("forceFaceLeft", false);
            animator.SetBool("forceFaceRight", false);
        }
    }
}
