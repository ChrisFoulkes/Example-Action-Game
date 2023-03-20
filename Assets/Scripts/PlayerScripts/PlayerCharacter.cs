using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerCharacter : MonoBehaviour, IDeath
{
    private bool isDead = false;
    private IMovement movementController;


    public void Start()
    {
        movementController = GetComponent<IMovement>();
    }

    public bool IsDead() 
    {
        return isDead;
    }

    public void StartDeath() 
    {
        isDead = true;
        GetComponent<Rigidbody2D>().simulated = false;

        PlayerDeathEvent pDeathEvent = new PlayerDeathEvent();
        pDeathEvent.FireEvent();
    }


    public void DeathAnimationComplete()
    {
        DisplayDeathUiEvent DeathUIEvent = new DisplayDeathUiEvent();
        DeathUIEvent.FireEvent();
    }
}