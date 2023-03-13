
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthEventArgs : EventArgs
{
    public float amount;

    public HealthEventArgs(float health)
    {
        this.amount = health;
    }
}

public interface IHealth
{
    public event EventHandler<HealthEventArgs> HealthChangedEvent;

    void ChangeHealth(float amount);

    float CurrentHealth();

    float GetMaxHP();
}
