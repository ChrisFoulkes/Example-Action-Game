using UnityEngine;

public class CharacterUpgradeHandler : MonoBehaviour
{

    private GameObject player;
    IMovement playerMovement;
    IHealth playerHealth;
    IHeal playerHealController;

    PlayerAbilityManager playerAbilityManager;

    CharacterStatsController characterStatsController;

    public void Initialize(GameObject playerObject)
    {
        player = playerObject;
        playerHealth = player.GetComponent<IHealth>();
        playerHealController = player.GetComponent<IHeal>();
        playerMovement = player.GetComponent<IMovement>();
        playerAbilityManager = player.GetComponentInChildren<PlayerAbilityManager>();
        characterStatsController = player.GetComponent<CharacterStatsController>();

    }

    void ChangeMovementSpeed(float amount)
    {
        playerMovement.ChangeSpeed(amount);
    }
    public void ApplyCharacterUpgrade(CharacterUpgradeTypes upgradeType, float amount, StatData stat)
    {
        switch (upgradeType)
        {
            case CharacterUpgradeTypes.speed:
                ChangeMovementSpeed(amount);
                break;
            case CharacterUpgradeTypes.heal:
                playerHealController.ApplyHealing(new HealInfo(5f));
                break;
            case CharacterUpgradeTypes.stat:
                if (stat != null)
                {
                    characterStatsController.AlterStat(stat.ID, amount);
                }
                break;
            default:
                Debug.LogWarning("Unsupported character upgrade type: " + upgradeType);
                break;
        }
    }

    //At some point lets remove the need for the is Checking 
    public void HandleUpgrade(UpgradeData uData)
    {
        int abilityID = -1;

        if (uData is ProjectileUpgradeData projectileUpgradeData)
        {
            abilityID = projectileUpgradeData.ability.AbilityID;

            foreach (ProjectileUpgradeEffect upgradeEffect in projectileUpgradeData.upgradeEffects)
            {
                playerAbilityManager.UpgradeAbility(abilityID, upgradeEffect);
            }
        }
        else if (uData is MeleeUpgradeData meleeUpgradeData)
        {
            abilityID = meleeUpgradeData.ability.AbilityID;

            foreach (MeleeUpgradeEffect upgradeEffect in meleeUpgradeData.upgradeEffects)
            {
                playerAbilityManager.UpgradeAbility(abilityID, upgradeEffect);
            }
        }
        else if (uData is CharacterUpgradeData characterUpgradeData)
        {
            foreach (CharacterUpgradeEffect upgradeEffect in characterUpgradeData.upgradeEffects)
            {
                ApplyCharacterUpgrade(upgradeEffect.upgradeType, upgradeEffect.amount, upgradeEffect.statData);
            }
        }
    }
}