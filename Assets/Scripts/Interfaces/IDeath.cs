
using System;

public interface IDeath
{
    void AddListener(GameEvent.EventDelegate<GameEvent> listener);
    void RemoveListener(GameEvent.EventDelegate<GameEvent> listener);

    void StartDeath();

    void CompleteDeath();

    bool IsDead();
}
