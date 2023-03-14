using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    private bool canMove = true;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerStopMovementEvent.RegisterListener(OnStopMovement);
        PlayerDeathEvent.RegisterListener(OnDeath);
    }

    private void OnDestroy()
    {

        PlayerStopMovementEvent.UnregisterListener(OnStopMovement);
        PlayerDeathEvent.RegisterListener(OnDeath);
    }

    public void CanMove(bool Move) 
    {
        canMove = Move;
    }
    public void ChangeSpeed(float amount)
    {
        if (moveSpeed < 15)
        {
            moveSpeed += amount;
            if (moveSpeed > 15) { moveSpeed = 15; }
        }
    }


    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        if (canMove)
        {
            rb.velocity = movement * moveSpeed;
        }
    }


    void OnDeath(PlayerDeathEvent deathEvent)
    {
        isDead = true;
    }

    void OnStopMovement(PlayerStopMovementEvent stopEvent)
    {
        StartCoroutine(StopMovementAnimation(stopEvent.duration));
    }

    // need to adjust this to not cause issues with the death disabling of moving
    private IEnumerator StopMovementAnimation(float stopDuration)
    {

        CanMove(false);
        yield return new WaitForSeconds(stopDuration);
        {
            if(!isDead)
                CanMove(true);
            
        }
    }
}
