public interface IHeal
{
    public void ApplyHealing(HealInfo healInfo);
    public void AddHealListener(GameEvent.EventDelegate<GameEvent> listener);
    public void RemoveHealListener(GameEvent.EventDelegate<GameEvent> listener);
}
