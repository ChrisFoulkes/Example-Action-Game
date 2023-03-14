using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerCharacter : MonoBehaviour, IDeath
{
    public Animator LevelUpEffect;
    private bool isDead = false;
    private FloatingCombatTextController combatText;
    private IMovement movementController;

    private float currentXP = 0;
    private float requiredXP = 10;
    private float level = 1;

    public void Start()
    {
        combatText = GetComponent<FloatingCombatTextController>();
        movementController= GetComponent<IMovement>(); 
        EnemyKilledEvent.RegisterListener(EnemyKilled);
    }

    void OnDestroy()
    {
        EnemyKilledEvent.UnregisterListener(EnemyKilled);
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

    public void EnemyKilled(EnemyKilledEvent killEvent) 
    {
        if (!isDead) {
            ChangeXP(killEvent.xpValue);
        }
    }


    public void ChangeXP(float amount)
    {
        bool isLevelUP = false;
        currentXP += amount;

        if (currentXP > requiredXP)
        {
            LevelUp();
            isLevelUP = true; 
            currentXP -= requiredXP;
        }

        PlayerExperienceEvent pXPEvent= new PlayerExperienceEvent(isLevelUP, requiredXP, currentXP);
        pXPEvent.FireEvent();
    }

    public void LevelUp() 
    {
        level++;
        LevelUpEffect.SetTrigger("LevelUp");
        combatText.CreateFloatingCombatText("+1 Level", Color.yellow, 0.65f);
    }
}