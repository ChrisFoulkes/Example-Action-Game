using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUpgradeHandler : MonoBehaviour
{

    private GameObject player;
    IMovement playerMovement;
    IHealth playerHealth;

    PlayerAbilityManager playerAbilityManager;

    public void Initialize(GameObject playerObject)
    {
        player = playerObject;
        playerHealth = player.GetComponent<IHealth>();
        playerMovement = player.GetComponent<IMovement>();
        playerAbilityManager = player.GetComponentInChildren<PlayerAbilityManager>();

    }

    void ChangeMovementSpeed(float amount)
    {
        playerMovement.ChangeSpeed(amount);
    }
    public void ApplyCharacterUpgrade(CharacterUpgradeTypes upgradeType, float amount)
    {
        switch (upgradeType)
        {
            case CharacterUpgradeTypes.speed:
                ChangeMovementSpeed(amount);
                break;
            case CharacterUpgradeTypes.heal:
                playerHealth.ChangeHealth(amount);
                break;
            default:
                Debug.LogWarning("Unsupported character upgrade type: " + upgradeType);
                break;
        }
    }

    public void HandleUpgrade(UpgradeData uData)
    {
        int abilityID = -1;

        if (uData is ProjectileUpgradeData projectileUpgradeData)
        {
            abilityID = projectileUpgradeData.ability.AbilityID; 
            
            foreach (UpgradeEffect upgradeEffect in projectileUpgradeData.upgradeEffects)
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
                ApplyCharacterUpgrade(upgradeEffect.upgradeType, upgradeEffect.amount);
            }
        }
    }
}