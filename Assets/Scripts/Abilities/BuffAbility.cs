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
    public PlayerAbilityManager caster;
    public CharacterStatsController casterStats;
    public BuffEffectController buffController;
    public BuffAbility(BuffData aData, PlayerAbilityManager playerAbilityManager) : base(aData)
    {
        buffData = new ActiveBuffData(aData.affectStats, aData.baseBuffDuration, aData.BuffID);

        castTime = aData.castTime;

        caster = playerAbilityManager;
        upgradeHandler = new BuffUpgradeHandler(this);

        //need to update the way we handle passing caster data this is dumb
        casterStats = caster.gameObject.GetComponent<CharacterStatsController>();
        buffController = caster.GetComponentInParent<BuffEffectController>();
    }

    public override void CastAbility()
    {
        if (castTime > cooldown) { adjustCooldowm(castTime); }

        buffController.ApplyBuff(buffData);


        //currently global events maybe should be local 
        PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = castTime;
        EventManager.Raise(stopMovementEvent);

        SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.getFacingDirection(caster.transform.position), castTime);
        EventManager.Raise(setDirectionEvent);
    }

    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        upgradeHandler.ApplyUpgrade(upgradeEffect);
    }
}
