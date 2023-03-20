using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperienceController : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float currentXP = 0;
    [SerializeField] private float baseXP = 100;
    [SerializeField] private float currentRequired;
    [SerializeField] private float xpMultiplier = 1.25f;

    private bool isLevelUP = false;

    public Animator LevelUpEffect;
    private bool isDead = false;
    private FloatingCombatTextController combatText;
    private IDeath deathController;

    public int CurrentLevel { get { return currentLevel; } }
    public float CurrentXP { get { return currentXP; } }
    public void Start()
    {
        deathController = GetComponent<IDeath>();
        combatText = GetComponent<FloatingCombatTextController>();
        EnemyKilledEvent.RegisterListener(EnemyKilled);
    }

    void OnDestroy()
    {
        EnemyKilledEvent.UnregisterListener(EnemyKilled);
    }

    private float CalculateXPForLevel(int level)
    {
        currentRequired = Mathf.RoundToInt(baseXP * Mathf.Pow(xpMultiplier, level - 1));
        return currentRequired;
    }

    public void AddExperience(float xp)
    {
        currentXP += xp;

        // Check if the player has enough XP to level up
        while (currentXP >= CalculateXPForLevel(currentLevel))
        {
            isLevelUP = true;
            LevelUp();
        }


        PlayerExperienceEvent pXPEvent = new PlayerExperienceEvent(isLevelUP, currentRequired, currentXP, CurrentLevel);
        pXPEvent.FireEvent();

        isLevelUP = false;
    }

    public void LevelUp()
    {            
        // Level up
        currentXP -= CalculateXPForLevel(currentLevel);
        currentLevel++;
        LevelUpEffect.SetTrigger("LevelUp");
        combatText.CreateFloatingCombatText("+1 Level", Color.yellow, false, 0.65f);
    }

    public void EnemyKilled(EnemyKilledEvent killEvent)
    {
        if (!deathController.IsDead())
        {
            AddExperience(killEvent.xpValue);
        }
    }
}
