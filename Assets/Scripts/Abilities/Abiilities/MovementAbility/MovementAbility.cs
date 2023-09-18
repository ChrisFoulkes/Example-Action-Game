using UnityEngine;


public class MovementAbility : Ability
{
    public float movementSpeed;
    public float movementDuration;
    public Vector3 maximumDistance = new Vector3(5f, 5f, 0f);
    public PlayerCasterContext caster;  

    public MovementAbility(MovementData aData, CasterContext caster) : base(aData)
    {
        this.caster = (PlayerCasterContext)caster;
        movementSpeed = aData.movementSpeed;
        movementDuration = aData.movementDuration;
    }

    protected override void OnCast()
    {
        Vector2 inputDirection = caster.PlayerMovement.movement;

        caster.PlayerMovement.StartCoroutine(caster.PlayerMovement.ApplyMovement(inputDirection, movementSpeed, movementDuration));
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        // Apply upgrade effects, e.g. increase movement speed, duration, etc.
    }
}

