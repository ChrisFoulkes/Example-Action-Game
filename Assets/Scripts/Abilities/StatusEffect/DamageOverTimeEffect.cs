using UnityEngine;


// Should the Scriptable Object Damage over time be handling the effects ?
[CreateAssetMenu(menuName = "Status Effects/Damage Over Time")]
public class DamageOverTimeEffect : StatusEffect
{
    public StatAssociation damagePerTick;
    public float chanceToApply;
    public bool canMultiApply = false;


    private PlayerCasterContext _caster;

    public override void ApplyEffect(StatusEffectController controller, CasterContext caster)
    {
        _caster = (PlayerCasterContext)caster;

        float randomNumber = Random.Range(0, 100);

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

        target.GetComponent<IDamage>().ApplyDamage(new DamageInfo(damage, false, FloatingColourType.Ignite), _caster);

        GameObject effectPrefab = Instantiate(statusPrefab, target.transform);
        effectPrefab.GetComponent<BurnEffect>().Initialize(activeStatusEffect.CountInstance, activeStatusEffect.BonusEffectActive);
        activeStatusEffect.EffectAsset = effectPrefab;
    }
}

