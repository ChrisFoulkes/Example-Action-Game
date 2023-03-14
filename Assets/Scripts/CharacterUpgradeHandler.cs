using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUpgradeHandler: MonoBehaviour
{

    private GameObject player;
    IMovement playerMovement;
    IHealth playerHealth;
    public CharacterUpgradeHandler(GameObject playerObject) 
    {
        player = playerObject;
        playerHealth = player.GetComponent<IHealth>();
        playerMovement = player.GetComponent<IMovement>();
    }

    void Start()
    {
    }

    void ChangeMovementSpeed(float amount)
    {
        playerMovement.ChangeSpeed(amount);
    }

    public void HandleUpgrade(CharacterUpgradeData uData) 
    {
        switch(uData.upgradeType)
        {
            case CharacterUpgradeTypes.speed:
                ChangeMovementSpeed(uData.amount);
                break;
            case CharacterUpgradeTypes.heal:
                playerHealth.ChangeHealth(uData.amount);
                break;
        }
    }
}
