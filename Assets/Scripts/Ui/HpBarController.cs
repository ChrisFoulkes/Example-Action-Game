using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarController : MonoBehaviour
{
    private float maxValue;
    private float currentValue;

    public SpriteRenderer amountSprite;

    IHealth healthController;

    private void Start()
    {
        healthController = GetComponentInParent<IHealth>();

        maxValue = healthController.GetMaxHP();
        currentValue = maxValue;
        healthController.HealthChangedEvent += OnHealthChange;
    }

    void OnHealthChange(object sender, HealthEventArgs changeEvent) 
    {
        currentValue += changeEvent.amount;

        UpdateHealthBar();
    }

    void UpdateHealthBar() 
    {
        float fillAmount = Mathf.Clamp(currentValue / maxValue, 0, 1);
        amountSprite.size = new Vector2(fillAmount, amountSprite.size.y);
    }
}
