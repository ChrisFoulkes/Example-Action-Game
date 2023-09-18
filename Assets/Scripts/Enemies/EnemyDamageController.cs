using EventCallbacks;
using UnityEngine;
public class EnemyDamageController : DamageController
{
    protected EnemyDamageEvent _damageEvent = new EnemyDamageEvent();
    protected EnemyHealEvent _healEvent = new EnemyHealEvent(); 
    
    protected override DamageEvent DamageEvent => _damageEvent;
    protected override HealEvent HealEvent => _healEvent;
    public override void AddDamageListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _damageEvent.AddLocalListener(listener);
    }

    public override void RemoveDamageListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _damageEvent.RemoveLocalListener(listener);
    }

    public override void AddHealListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _healEvent.AddLocalListener(listener);
    }

    public override void RemoveHealListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        _healEvent.RemoveLocalListener(listener);
    }

    public override void ApplyDamage(DamageInfo damageInfo, CasterContext caster)
    {
        DamageInfo AdjustedDamage = ApplyDefense(damageInfo);

        _damageEvent.DamageInfo = AdjustedDamage;
        _damageEvent.Caster = (PlayerCasterContext)caster;
        _damageEvent.Target = new TargetContext(transform);

        if (AdjustedDamage.Amount < 0)
        {
            EventManager.Raise(_damageEvent);
        }
    }
}
