using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementAbility : Ability
{
    public float movementSpeed;
    public float movementDuration;
    public Vector3 maximumDistance = new Vector3(5f, 5f, 0f);

    private Transform caster;
    private Rigidbody2D casterRigidbody; 
    private PlayerMovement playerMovement;


    public MovementAbility(MovementData aData, Transform casterTransform) : base(aData)
    {
        movementSpeed = aData.movementSpeed;
        movementDuration = aData.movementDuration;

        caster = casterTransform;
        casterRigidbody = caster.GetComponent<Rigidbody2D>();
        playerMovement = caster.GetComponent<PlayerMovement>();
    }

    public override void CastAbility()
    {
        if (IsCastable())
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;

            playerMovement.StartCoroutine(playerMovement.ApplyMovement(inputDirection, movementSpeed, movementDuration));
        }
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        // Apply upgrade effects, e.g. increase movement speed, duration, etc.
    }
}

