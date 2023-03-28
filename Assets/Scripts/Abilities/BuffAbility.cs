using EventCallbacks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public class BuffUpgradeHandler : UpgradeHandler
{
    private BuffAbility parentAbility;

    public BuffUpgradeHandler(BuffAbility parentAbility)
    {
        this.parentAbility = parentAbility;
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        /*
         * replace once we have upgrades for buffs
        if (upgradeEffect is MeleeUpgradeEffect meleeUpgradeEffect)
        {
            switch (meleeUpgradeEffect.upgradeType)
            {
            }
        }
        */
    }
}

public class ActiveBuffData
{
    public int BuffID;
    public List<AffectStat> affectStats = new List<AffectStat>();
    public float buffDuration;

    public ActiveBuffData(List<AffectStat> affectStats, float duration, int ID) 
    {
        BuffID = ID;
        buffDuration = duration;
        this.affectStats = affectStats;
    }
}

public class BuffAbility : Ability
{
    public ActiveBuffData buffData;
    AbilityContext caster;

    public BuffAbility(BuffData aData, AbilityContext caster) : base(aData)
    {
        this.caster = caster;

        buffData = new ActiveBuffData(aData.affectStats, aData.baseBuffDuration, aData.BuffID);
        castTime = aData.castTime;

        upgradeHandler = new BuffUpgradeHandler(this);

    }

    public override void CastAbility()
    {
        if (castTime > cooldown) { AdjustCooldown(castTime); }

        caster.BuffEffectController.ApplyBuff(buffData);


        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(caster.transform.position), castTime);
        EventManager.Raise(setDirectionEvent);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        upgradeHandler.ApplyUpgrade(upgradeEffect);
    }
}
