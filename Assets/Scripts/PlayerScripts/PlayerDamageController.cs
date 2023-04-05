using EventCallbacks;
using UnityEngine.UIElements;

public class PlayerDamageController : DamageController
{
    protected PlayerDamageEvent _damageEvent = new PlayerDamageEvent();
    protected PlayerHealEvent _healEvent = new PlayerHealEvent();

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
    public override void ApplyDamage(DamageInfo damageInfo, AbilityCasterContext caster)
    {
        DamageInfo AdjustedDamage = ApplyDefense(damageInfo);

        _damageEvent.DamageInfo = AdjustedDamage;
        _damageEvent.Caster = (EnemyCasterContext)caster;
        _damageEvent.Target = new TargetContext(transform);

        if (AdjustedDamage.Amount < 0)
        {
            EventManager.Raise(_damageEvent);
        }
    }
}