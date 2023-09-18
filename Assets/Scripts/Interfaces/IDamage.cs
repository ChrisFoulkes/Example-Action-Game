public interface IDamage
{
    public void ApplyDamage(DamageInfo damageInfo, CasterContext caster = null);
    public void AddDamageListener(GameEvent.EventDelegate<GameEvent> listener);
    public void RemoveDamageListener(GameEvent.EventDelegate<GameEvent> listener);

    public TargetContext GetContext();

}
