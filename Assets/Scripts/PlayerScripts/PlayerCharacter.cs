using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerCharacter : MonoBehaviour, IDeath
{
    public HpBarController HpBar;
    public Color startColor;
    public Color endColor;
    public float blendDuration;
    public Animator LevelUpEffect;
    private Pathfinding.AIPath aiPath;
    private bool isDead = false;
    private FloatingCombatTextController combatText;

    private float currentXP = 0;
    private float requiredXP = 10;
    private float level = 1;

    public void Start()
    {
        aiPath = GetComponent<Pathfinding.AIPath>();
        combatText = GetComponent<FloatingCombatTextController>();

        PlayerStopMovementEvent.RegisterListener(OnStopMovement);
        EnemyKilledEvent.RegisterListener(EnemyKilled);
    }

    void OnDestroy()
    {
        EnemyKilledEvent.UnregisterListener(EnemyKilled);
        PlayerStopMovementEvent.UnregisterListener(OnStopMovement);
    }

    private void CanPlayerMove(bool canMove) 
    {
        aiPath.canMove = canMove;
    }


    public void StartDeath() 
    {
        isDead = true;
        CanPlayerMove(false);
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

    void OnStopMovement(PlayerStopMovementEvent stopEvent)
    {
        StartCoroutine(StopMovementAnimation(stopEvent.duration));
    }

    // need to adjust this to not cause issues with the death disabling of moving
    private IEnumerator StopMovementAnimation(float stopDuration)
    {

        CanPlayerMove(false); 
        yield return new WaitForSeconds(stopDuration);
        if (!isDead)
        {
            CanPlayerMove(true);
        }
    }
}