public interface IDamage
{
    public void ApplyDamage(DamageInfo damageInfo, AbilityCasterContext caster = null);
    public void AddDamageListener(GameEvent.EventDelegate<GameEvent> listener);
    public void RemoveDamageListener(GameEvent.EventDelegate<GameEvent> listener);

}
