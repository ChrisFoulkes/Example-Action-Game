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
    }

    private void OnEnable()
    {

        EventManager.AddGlobalListener<PlayerStopMovementEvent>(OnStopMovement);
        EventManager.AddGlobalListener<PlayerDeathEvent>(OnDeath);
    }

    private void OnDisable()
    {   

        EventManager.RemoveGlobalListener<PlayerStopMovementEvent>(OnStopMovement);
        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnDeath);
    }

    public void CanMove(bool Move) 
    {
        canMove = Move;

        if(!canMove)
        {

            rb.velocity = Vector3.zero;
        }
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
