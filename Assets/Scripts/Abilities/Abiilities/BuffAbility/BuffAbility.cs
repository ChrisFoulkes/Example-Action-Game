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

    public BuffAbility(BuffAbilityData abilityData, CasterContext caster) : base(abilityData)
    {
        _caster = (PlayerCasterContext)caster;

        foreach (BuffData data in abilityData.BuffData) 
        {
            _buffData.Add(new ActiveBuffData(data.AffectedStats, data.BuffDuration, data.BuffID, data.isStackable, data.maxStacks));
        }

        CastTime = abilityData.castTime;

        AbilityUpgradeHandler = new BuffUpgradeHandler(this);
    }

    protected override void OnCast()
    {
        foreach (ActiveBuffData buff in _buffData) 
        {
            _caster.BuffController.ApplyBuff(buff);
        }
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        AbilityUpgradeHandler.ApplyUpgrade(upgradeEffect);
    }
}
