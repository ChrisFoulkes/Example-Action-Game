
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using EventCallbacks;

public class HealthController : MonoBehaviour, IHealth
{
    public event EventHandler<HealthEventArgs> HealthChangedEvent;

    public float maximumHealth = 20;

    [SerializeField]
    private float _currentHP;
    private FloatingCombatTextController combatTextController;
    public Color startColor;
    public Color endColor;
    public float blendDuration;
    private SpriteRenderer sprite;
    IDeath deathHandler;

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        deathHandler = GetComponent<IDeath>();
        _currentHP = maximumHealth;
        combatTextController = GetComponent<FloatingCombatTextController>();
    }

    public void ChangeHealth(float amount)
    {
        //need to implement system to handle amount for damage vs healing changed event should not be - AMOUNT 
        HealthChangedEvent(this, new HealthEventArgs(amount));

        if (_currentHP > 0)
        {
            if (amount > 0)
            {
                combatTextController.CreateFloatingCombatText("+" + amount, Color.green);
            }
            else
            {
                combatTextController.CreateFloatingCombatText(amount.ToString(), Color.red);
            }
        }

        _currentHP += amount;

        if(_currentHP > maximumHealth) 
        {
            _currentHP = maximumHealth;
        }
        if (_currentHP <= 0)
        {
            deathHandler.StartDeath();
        }

        FlashColour();
    }

    public void Kill()
    {
        HealthChangedEvent(this, new HealthEventArgs(-(maximumHealth - _currentHP)));
        _currentHP = 0; 

        FlashColour();


        deathHandler.StartDeath();
    }

    public float CurrentHealth() 
    {
        return _currentHP;
    }

    public float GetMaxHP() 
    {
        return maximumHealth;
    }

    private void FlashColour()
    {
        sprite.color = endColor;
        StopCoroutine(BlendColors());
        StartCoroutine(BlendColors());
    }


    private IEnumerator BlendColors()
    {
        float elapsedTime = 0f;
        while (elapsedTime < blendDuration)
        {
            sprite.color = Color.Lerp(startColor, endColor, elapsedTime / blendDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = endColor;
    }

}
