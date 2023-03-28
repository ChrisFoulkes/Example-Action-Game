using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Should the Scriptable Object Damage over time be handling the effects ?
[CreateAssetMenu(menuName = "Status Effects/Damage Over Time")]
public class DamageOverTimeEffect : StatusEffect
{
    public StatAssociation damagePerTick;
    public float chanceToApply;
    public bool canMultiApply = false;


    private AbilityContext _caster;

    public override void ApplyEffect(StatusEffectController controller, AbilityContext caster)
    {
        _caster = caster;

        float randomNumber = UnityEngine.Random.Range(0, 100);

        if (randomNumber < chanceToApply)
        {
            controller.TryApplyStatusEffect(this, _caster);
        }
    }


    public override void RemoveEffect(GameObject target)
    {

    }

    public override void UpdateEffect(GameObject target, ActiveStatusEffect activeStatusEffect)
    {
        float damage = damagePerTick.CalculateModifiedValue(_caster.CharacterStatsController) * activeStatusEffect.CountInstance;
        if (activeStatusEffect.BonusEffectActive) 
        {
            damage *= 2;
        }
        target.GetComponent<IHealth>().ChangeHealth(damage, false, FloatingColourType.Ignite);

        GameObject effectPrefab = Instantiate(statusPrefab, target.transform);

        activeStatusEffect.EffectAsset = effectPrefab;
    }
}

