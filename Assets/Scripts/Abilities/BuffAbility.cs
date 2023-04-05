using EventCallbacks;
using System.Collections.Generic;


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
    private List<ActiveBuffData> _buffData = new List<ActiveBuffData>();
    private readonly PlayerCasterContext _caster;

    public BuffAbility(BuffAbilityData abilityData, AbilityCasterContext caster) : base(abilityData)
    {
        _caster = (PlayerCasterContext)caster;

        foreach (BuffData data in abilityData.BuffData) 
        {
            _buffData.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID));
        }

        CastTime = abilityData.castTime;

        AbilityUpgradeHandler = new BuffUpgradeHandler(this);
    }

    public override void CastAbility()
    {

        foreach (ActiveBuffData buff in _buffData) 
        {
            _caster.BuffController.ApplyBuff(buff);
        }

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
