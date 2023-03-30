using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AppliedStatus
{
    public Transform enemy;
    public ActiveStatusEffect effect;

    public AppliedStatus(Transform enemy, ActiveStatusEffect effect)
    {
        this.enemy = enemy;
        this.effect = effect;
    }
}

public class PlayerStatusTracker : MonoBehaviour
{
    public List<AppliedStatus> appliedStatusEffects = new List<AppliedStatus>();

    public void AddStatusEffect(AppliedStatus appliedStatus)
    {
        appliedStatusEffects.Add(appliedStatus);
    }

    public void RemoveStatusEffect(AppliedStatus appliedStatus)
    {
        appliedStatusEffects.Remove(appliedStatus);
    }
    public void RemoveStatusEffectsFromTarget(Transform target)
    {
        appliedStatusEffects.RemoveAll(status => status.enemy == target);

    }
}
