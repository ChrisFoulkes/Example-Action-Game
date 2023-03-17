using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBarController : MonoBehaviour
{
    private float maxValue;
    private float currentValue;

    public Image amountSprite;
    public TextMeshProUGUI hpText;

    public HealthController healthController;

    private void Start()
    {

        maxValue = healthController.GetMaxHP();
        currentValue = maxValue;
        healthController.HealthChangedEvent += OnHealthChange;
        hpText.text = currentValue + "/" + maxValue;
    }

    void OnHealthChange(object sender, HealthEventArgs changeEvent)
    {
        currentValue += changeEvent.amount;
        if (currentValue > maxValue) 
        {
            currentValue = maxValue;
        }
        hpText.text = currentValue + "/" + maxValue;
        UpdateHealthBar();
    }


    void UpdateHealthBar()
    {
        float fillAmount = Mathf.Clamp(currentValue / maxValue, 0, 1);
        Vector3 xpScale = amountSprite.transform.localScale;
        amountSprite.transform.localScale = new Vector3(fillAmount, xpScale.y, xpScale.z);
    }
}
