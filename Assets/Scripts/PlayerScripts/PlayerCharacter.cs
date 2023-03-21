using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerCharacter : MonoBehaviour, IDeath
{
    private bool isDead = false;
    private IMovement movementController;

    PlayerDeathEvent pDeathEvent = new PlayerDeathEvent();

    public void Start()
    {
        movementController = GetComponent<IMovement>();
    }
    public void AddListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        pDeathEvent.AddLocalListener(listener);
    }

    public void RemoveListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        pDeathEvent.RemoveLocalListener(listener);
    }
    public bool IsDead() 
    {
        return isDead;
    }

    public void StartDeath() 
    {
        isDead = true;
        GetComponent<Rigidbody2D>().simulated = false;
        EventManager.Raise(pDeathEvent);
    }


    public void CompleteDeath()
    {
        DisplayDeathUiEvent DeathUIEvent = new DisplayDeathUiEvent();
        EventManager.Raise(DeathUIEvent);
    }
}