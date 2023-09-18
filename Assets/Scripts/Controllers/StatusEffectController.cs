using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStatusEffect
{
    public StatusEffect StatusEffect;
    public PlayerCasterContext AppliedBy;

    public GameObject EffectAsset;
    public float ElapsedTime;

    public AppliedStatus AppliedStatus;

    public StatusEffectController Controller;
    public Coroutine ActiveCoroutine;
    public Coroutine TickCoroutine;

    public int CountInstance = 0;
    public bool BonusEffectActive = false;

    public ActiveStatusEffect(StatusEffectController controller, StatusEffect statusEffect, CasterContext appliedBy, float time)
    {
        Controller = controller;
        ElapsedTime = time;
        StatusEffect = statusEffect;
        AppliedBy = (PlayerCasterContext)appliedBy;
    }
}

public class StatusEffectController : MonoBehaviour
{
    private IDeath deathController;

    public List<ActiveStatusEffect> activeEffects = new List<ActiveStatusEffect>();

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

    public void TryApplyStatusEffect(StatusEffect statusEffect, PlayerCasterContext appliedBy)
    {
        if (deathController.IsDead())
        {
            return;
        }

        ActiveStatusEffect existingEffect = activeEffects.Find(effect => effect.StatusEffect.name == statusEffect.name);

        if (existingEffect != null)
        {
            //if status effect already exists reset duration timer 
            existingEffect.ElapsedTime = 0f;
            existingEffect.CountInstance++;
            if (existingEffect.CountInstance > 4)
            {
                existingEffect.CountInstance = 4;
                existingEffect.BonusEffectActive = true;
            }
        }
        else
        {
            ActiveStatusEffect newStatusEffect = new ActiveStatusEffect(this, statusEffect, appliedBy, 0f);
            AppliedStatus newAppliedStatus = new AppliedStatus(transform, newStatusEffect);

            newStatusEffect.AppliedStatus = newAppliedStatus;
            appliedBy.PlayerStatusTracker.AddStatusEffect(newAppliedStatus);
            activeEffects.Add(newStatusEffect);
            newStatusEffect.ActiveCoroutine = StartCoroutine(ApplyStatusEffectCoroutine(newStatusEffect));

            newStatusEffect.CountInstance = 1;
            newStatusEffect.BonusEffectActive = false;


        }
    }

    private IEnumerator ApplyStatusEffectCoroutine(ActiveStatusEffect activeStatusEffect)
    {

        if (activeStatusEffect.StatusEffect.tickRate > 0)
        {
            activeStatusEffect.TickCoroutine = StartCoroutine(ApplyTickEffectCoroutine(activeStatusEffect));
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

        RemoveStatusEffect(activeStatusEffect);
    }

    private IEnumerator ApplyTickEffectCoroutine(ActiveStatusEffect activeStatusEffect)
    {
        while (true)
        {
            activeStatusEffect.StatusEffect.UpdateEffect(gameObject, activeStatusEffect);
            yield return new WaitForSeconds(activeStatusEffect.StatusEffect.tickRate);
        }
    }

    public void RemoveStatusEffect(ActiveStatusEffect activeStatusEffect)
    {
        // Stop the coroutines
        StopCoroutine(activeStatusEffect.ActiveCoroutine);

        if (activeStatusEffect.StatusEffect.tickRate > 0)
        {
            StopCoroutine(activeStatusEffect.TickCoroutine);
        }

        // Remove the effect asset if available
        if (activeStatusEffect.EffectAsset != null)
        {
            Destroy(activeStatusEffect.EffectAsset);
        }

        // Remove the ActiveStatusEffect
        activeStatusEffect.AppliedBy.PlayerStatusTracker.RemoveStatusEffect(activeStatusEffect.AppliedStatus);
        activeEffects.Remove(activeStatusEffect);
    }

    private void OnDeath(GameEvent gameEvent)
    {

        // Loop through all active status effects
        foreach (ActiveStatusEffect activeStatusEffect in activeEffects)
        {
            // Remove the status effect from the corresponding player's status tracker
            activeStatusEffect.AppliedBy.PlayerStatusTracker.RemoveStatusEffectsFromTarget(transform);

            // Remove the effect asset if available
            if (activeStatusEffect.EffectAsset != null)
            {
                Destroy(activeStatusEffect.EffectAsset);
            }
        }

        // Clear the active effects list
        activeEffects.Clear();
    }
}