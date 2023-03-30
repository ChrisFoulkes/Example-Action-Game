using EventCallbacks;
using UnityEngine;

public abstract class DamageController : MonoBehaviour, IDamage, IHeal
{
    protected abstract DamageEvent DamageEvent { get; }
    protected abstract HealEvent HealEvent { get; }


    public abstract void AddDamageListener(GameEvent.EventDelegate<GameEvent> listener);

    public abstract void RemoveDamageListener(GameEvent.EventDelegate<GameEvent> listener);

    public abstract void AddHealListener(GameEvent.EventDelegate<GameEvent> listener);


    public abstract void RemoveHealListener(GameEvent.EventDelegate<GameEvent> listener);


    public abstract void ApplyDamage(DamageInfo damageInfo, AbilityCasterContext caster);


    public virtual void ApplyHealing(HealInfo healInfo)
    {
        HealEvent.HealInfo = healInfo;
        HealEvent.Target = new TargetContext(transform);

        if (healInfo.Amount > 0)
        {
            EventManager.Raise(HealEvent);
        }
    }

    protected virtual DamageInfo ApplyDefense(DamageInfo damageInfo)
    {
        DamageInfo AdjustedDamage = damageInfo;
        return AdjustedDamage;
    }
}
