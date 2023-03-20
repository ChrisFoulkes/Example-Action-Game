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

    public void HandleCharacterUpgrade(CharacterUpgradeData uData)
    {
        switch (uData.upgradeType)
        {
            case CharacterUpgradeTypes.speed:
                ChangeMovementSpeed(uData.amount);
                break;
            case CharacterUpgradeTypes.heal:
                playerHealth.ChangeHealth(uData.amount);
                break;
        }
    }

    public void HandleAttackUpgrade(ProjectileUpgradeData uData)
    {
        foreach(UpgradeEffect upgrade in uData.upgradeEffects)
        {
            playerAbilityManager.UpgradeAbility(0, upgrade.upgradeType, upgrade.amount);
        }
    }
}