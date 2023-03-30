public interface IHealth
{

    void AddListener(GameEvent.EventDelegate<GameEvent> listener);
    void RemoveListener(GameEvent.EventDelegate<GameEvent> listener);

    //void ChangeHealth(float amount, bool isCriticalHit = false, FloatingColourType colourType = FloatingColourType.Generic);

    float CurrentHealth();

    float GetMaxHP();
}
