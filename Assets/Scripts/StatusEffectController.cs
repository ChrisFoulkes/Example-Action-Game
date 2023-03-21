using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveStatusEffect
{
    public StatusEffect StatusEffect;
    public Coroutine ActiveCoroutine;
    public Coroutine ActiveTickCoroutine;
    public GameObject effectAsset;
    public float ElapsedTime;
}

public class StatusEffectController : MonoBehaviour
{
    private IDeath deathController;
    private List<ActiveStatusEffect> activeEffects = new List<ActiveStatusEffect>();

    private void Awake()
    {
        deathController = GetComponent<IDeath>();
    }

    private void OnEnable()
    {
        deathController.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        deathController.RemoveListener(OnDeath);
    }

    public void ApplyStatusEffect(StatusEffect statusEffect, bool canHaveMultipleInstances)
    {
        if (deathController.IsDead()) return;

        ActiveStatusEffect existingEffect = null;
        if (!canHaveMultipleInstances)
        {
            existingEffect = activeEffects.Find(effect => effect.StatusEffect.name == statusEffect.name);
        }

        if (existingEffect != null)
        {
            // What to do if multiple existing versions 
            existingEffect.ElapsedTime = 0f;
        }
        else
        {
            ActiveStatusEffect newEffect = new ActiveStatusEffect { StatusEffect = statusEffect, ElapsedTime = 0f };
            newEffect.ActiveCoroutine = StartCoroutine(ApplyStatusEffectCoroutine(newEffect));
            activeEffects.Add(newEffect);
        }
    }

    private IEnumerator ApplyStatusEffectCoroutine(ActiveStatusEffect activeStatusEffect)
    {
        if (activeStatusEffect.StatusEffect.tickRate > 0)
        {
            activeStatusEffect.ActiveTickCoroutine = StartCoroutine(ApplyTickEffectCoroutine(activeStatusEffect));
        }
        else
        {
            activeStatusEffect.StatusEffect.UpdateEffect(gameObject, activeStatusEffect);
        }

        while (activeStatusEffect.ElapsedTime < activeStatusEffect.StatusEffect.duration)
        {
            activeStatusEffect.ElapsedTime += Time.deltaTime;
            yield return null;
        }

        if (activeStatusEffect.ActiveTickCoroutine != null)
        {
            StopCoroutine(activeStatusEffect.ActiveTickCoroutine);
        }

        activeStatusEffect.StatusEffect.RemoveEffect(gameObject);
        activeEffects.Remove(activeStatusEffect);
    }

    private IEnumerator ApplyTickEffectCoroutine(ActiveStatusEffect activeStatusEffect)
    {
        while (true)
        {
            activeStatusEffect.StatusEffect.UpdateEffect(gameObject, activeStatusEffect);
            yield return new WaitForSeconds(activeStatusEffect.StatusEffect.tickRate);
        }
    }

    private void ClearStatusEffects()
    {
        foreach (ActiveStatusEffect activeStatus in activeEffects)
        {
            if (activeStatus.ActiveTickCoroutine != null)
            {
                StopCoroutine(activeStatus.ActiveTickCoroutine);
            }

            StopCoroutine(activeStatus.ActiveCoroutine);
            Destroy(activeStatus.effectAsset);
        }

        activeEffects.Clear();
    }

    private void OnDeath(GameEvent gameEvent)
    {
        ClearStatusEffects();
    }
}