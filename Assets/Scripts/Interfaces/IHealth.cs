
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IHealth {

    void AddListener(GameEvent.EventDelegate<GameEvent> listener);
    void RemoveListener(GameEvent.EventDelegate<GameEvent> listener);

    void ChangeHealth(float amount, FloatingColourType colourType = FloatingColourType.Generic);

    float CurrentHealth();

    float GetMaxHP();
}
