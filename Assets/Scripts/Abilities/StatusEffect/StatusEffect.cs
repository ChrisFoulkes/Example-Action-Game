using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string statusName;
    public float duration;
    public float tickRate;

    public GameObject statusPrefab;
    public abstract void ApplyEffect(StatusEffectController controller, CasterContext caster);

    public abstract void RemoveEffect(GameObject target);
    public abstract void UpdateEffect(GameObject target, ActiveStatusEffect activeStatusEffect);
}