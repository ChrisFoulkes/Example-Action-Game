using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string statusName;
    public float duration;
    public float tickRate;

    public GameObject statusPrefab;
    public abstract void ApplyEffect(StatusEffectController controller, AbilityContext caster);

    public abstract void RemoveEffect(GameObject target);
    public abstract void UpdateEffect(GameObject target, ActiveStatusEffect activeStatusEffect);
}