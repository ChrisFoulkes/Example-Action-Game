using System.Collections.Generic;
using UnityEngine;

public class CombustionAbility : HitAbility
{
    private PlayerCasterContext _caster;
    public StatAssociation combustionDamage;
    private GameObject effectPrefab;

    public CombustionAbility(CombustionData aData, CasterContext caster) : base(aData)
    {
        combustionDamage = aData.combustionDamage;
        hitStun = aData.hitStun;
        knockbackData = aData.knockbackData;
        _caster = (PlayerCasterContext)caster;
        effectPrefab = aData.effectObject;
    }

    protected override void OnCast() // this needs cleaning up 
    {
        // Create a copy of the statusEffects list to iterate through safely
        List<AppliedStatus> statusEffectsCopy = new List<AppliedStatus>(_caster.PlayerStatusTracker.appliedStatusEffects);

        // Create a new list to store the status effects to remove (to avoid modifying the original list during iteration)
        List<AppliedStatus> statusEffectsToRemove = new List<AppliedStatus>();

        foreach (AppliedStatus appliedEffect in statusEffectsCopy)
        {
            if (appliedEffect.effect.StatusEffect is DamageOverTimeEffect)
            {
                if (appliedEffect.effect.StatusEffect.statusName == "Ignite")
                {
                    IDamage damageController = appliedEffect.enemy.GetComponent<IDamage>();

                    float remainingDuration = appliedEffect.effect.StatusEffect.duration - appliedEffect.effect.ElapsedTime;
                    float remainingTicks = remainingDuration / appliedEffect.effect.StatusEffect.tickRate;
                    float baseDamage = 1 * appliedEffect.effect.CountInstance;
                    float finalDamage = baseDamage * Mathf.FloorToInt(remainingTicks);
                    if (appliedEffect.effect.BonusEffectActive)
                    {
                        finalDamage *= 2;
                    }

                    combustionDamage.baseValue = -finalDamage;

                    damageController.ApplyDamage(new DamageInfo(Mathf.RoundToInt(combustionDamage.CalculateModifiedValue(_caster.CharacterStatsController)), false, FloatingColourType.Ignite, hitStun, knockbackData), _caster);

                    GameObject combustionEffect = Object.Instantiate(effectPrefab, appliedEffect.enemy);
                    combustionEffect.GetComponent<BurnEffect>().Initialize(0, appliedEffect.effect.BonusEffectActive);
                    Vector2 knockbackDirection = (appliedEffect.enemy.position - _caster.transform.position).normalized;

                    appliedEffect.enemy.GetComponent<EnemyMovementController>().HandleKnockback(knockbackDirection, new Vector2(0.5f, 0.5f), 0.3f);


                    // Add the effect to the list of effects to remove
                    statusEffectsToRemove.Add(appliedEffect);
                }
            }
        }

        // Remove the status effects from the player's status tracker
        foreach (AppliedStatus effectToRemove in statusEffectsToRemove)
        {
            effectToRemove.effect.Controller.RemoveStatusEffect(effectToRemove.effect);
        }

    }

    public override void OnHit(TargetContext target, Attack attack) { } // not sure i like this (combustion isnt really a "hit" ability 


    public override void ApplyUpgrade(UpgradeEffect upgradeEffect)
    {
        AbilityUpgradeHandler.ApplyUpgrade(upgradeEffect);
    }

}