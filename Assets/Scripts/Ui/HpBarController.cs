using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    private float maxValue;
    private float currentValue;

    public Image amountSprite;
    public TextMeshProUGUI hpText;

    public HealthController healthController;

    void OnEnable()
    {
        healthController.AddListener(OnHealthChange);
    }

    void OnDisable()
    {
        healthController.RemoveListener(OnHealthChange);
    }

    private void Start()
    {

        maxValue = healthController.GetMaxHP();
        currentValue = healthController.CurrentHealth();
        hpText.text = currentValue + "/" + maxValue;
    }

    void OnHealthChange(GameEvent changeEvent)
    {
        HealthChangedEvent healthEvent = changeEvent as HealthChangedEvent;

        currentValue = healthController.CurrentHealth();
        maxValue = healthController.GetMaxHP();

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
