using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Damage Over Time")]
public class DamageOverTimeEffect : StatusEffect
{
    public StatAssociation damagePerTick;
    public float chanceToApply;
    public bool canMultiApply = false;

    public GameObject caster;
    public CharacterStatsController characterStatsController;

    public override void ApplyEffect(StatusEffectController controller, GameObject caster)
    {
        this.caster = caster;
        characterStatsController = caster.GetComponentInChildren<CharacterStatsController>();

        float randomNumber = Random.Range(0, 100);

        if (randomNumber < chanceToApply)
        {
            controller.ApplyStatusEffect(this, canMultiApply);
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        // Clean up if necessary
    }

    public override void UpdateEffect(GameObject target, ActiveStatusEffect activeStatusEffect)
    {
        target.GetComponent<IHealth>().ChangeHealth(damagePerTick.CalculateModifiedValue(characterStatsController), false, FloatingColourType.Ignite);

        GameObject effectPrefab = Instantiate(statusPrefab, target.transform);

        activeStatusEffect.effectAsset = effectPrefab;
    }
}

