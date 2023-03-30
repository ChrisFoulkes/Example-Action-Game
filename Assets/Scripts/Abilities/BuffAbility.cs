using EventCallbacks;

public class BuffUpgradeHandler : UpgradeHandler
{
    private readonly BuffAbility _parentAbility;

    public BuffUpgradeHandler(BuffAbility parentAbility)
    {
        _parentAbility = parentAbility;
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        // Implement the buff upgrade logic here
    }
}

public class BuffAbility : Ability
{
    private readonly ActiveBuffData _buffData;
    private readonly PlayerCasterContext _caster;

    public BuffAbility(BuffAbilityData abilityData, AbilityCasterContext caster) : base(abilityData)
    {
        _caster = (PlayerCasterContext)caster;
        _buffData = new ActiveBuffData(abilityData.AffectedStats, abilityData.BaseBuffDuration, abilityData.BuffID);
        CastTime = abilityData.castTime;

        AbilityUpgradeHandler = new BuffUpgradeHandler(this);
    }

    public override void CastAbility()
    {
        if (CastTime > Cooldown)
        {
            AdjustCooldown(CastTime);
        }

        _caster.BuffController.ApplyBuff(_buffData);

        // Currently global events, maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent { duration = CastTime };
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(_caster.transform.position), CastTime);
        EventManager.Raise(setDirectionEvent);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        AbilityUpgradeHandler.ApplyUpgrade(upgradeEffect);
    }
}
