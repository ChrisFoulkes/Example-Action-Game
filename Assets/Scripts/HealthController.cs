
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using EventCallbacks;

public class HealthController : MonoBehaviour, IHealth
{
    //needs to be replaced with event syste
    public bool shouldUpdateHealthBar;

    [SerializeField] private float maximumHealth = 20;


    [SerializeField]
    private float _currentHP;
    public Color startColor;
    public Color endColor;
    public float blendDuration;
    public SpriteRenderer sprite;
    IDeath deathHandler;

    private HealthChangedEvent healthChangedEvent = new HealthChangedEvent();

    public void Awake()
    {
        deathHandler = GetComponent<IDeath>();
    }


    public void Initialize(float initialHealth)
    {
        maximumHealth = initialHealth;
        _currentHP = maximumHealth;

    }

    public void AddListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        healthChangedEvent.AddLocalListener(listener);
    }

    public void RemoveListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        healthChangedEvent.RemoveLocalListener(listener);
    }

    public virtual void ChangeHealth(float amount, bool isCriticalHit = false, FloatingColourType colourType = FloatingColourType.Generic)
    {
        if (_currentHP > 0)
        {
            if (isCriticalHit) 
            {
                GenerateCombatText(amount, FloatingColourType.levelUp, true);
            }
            else
            {
                GenerateCombatText(amount, colourType);
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

        healthChangedEvent.ChangeValue = amount;
        healthChangedEvent.RaiseLocal();

        FlashColour();
    }

    protected virtual void GenerateCombatText(float amount, FloatingColourType colourType, bool isCriticalHit = false)
    {
        if (amount > 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText("+" + amount, transform, FloatingColourType.Heal, false);
        }
        else if (amount < 0)
        {
            if (isCriticalHit)
            {

                FloatingCombatTextController.Instance.CreateFloatingCombatText(Mathf.Abs(amount).ToString(), transform, colourType, false, 0.9f, 0.8f);
            }
            else 
            {
                FloatingCombatTextController.Instance.CreateFloatingCombatText(Mathf.Abs(amount).ToString(), transform, colourType, false);
            }
        }
    }

    public void Kill()
    {
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

            if (deathHandler.IsDead()) 
            {
                blendDuration = 0f;
                sprite.color = endColor;
            }
        }

        sprite.color = endColor;
    }

}
