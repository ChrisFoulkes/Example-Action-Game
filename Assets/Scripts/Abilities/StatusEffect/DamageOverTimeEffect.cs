using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Damage Over Time")]
public class DamageOverTimeEffect : StatusEffect
{
    public float damagePerTick;
    public float chanceToApply;
    public bool canMultiApply = false;

    public override void ApplyEffect(StatusEffectController controller)
    {
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
        target.GetComponent<IHealth>().ChangeHealth(damagePerTick, false, FloatingColourType.Ignite);

        GameObject effectPrefab = Instantiate(statusPrefab, target.transform);

        activeStatusEffect.effectAsset = effectPrefab;
    }
}

