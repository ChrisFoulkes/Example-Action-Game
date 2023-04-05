using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] private float maxValue;
    [SerializeField] private float currentValue;

    public Image amountSprite;
    public TextMeshProUGUI hpText;

    public HealthController healthController;

    void OnEnable()
    {
        if (healthController != null)
        {
            healthController.AddListener(OnHealthChange);
        }
    }

    void OnDisable()
    {
        healthController.RemoveListener(OnHealthChange);
    }

    private void Start()
    {
        if (healthController != null)
        {
            maxValue = healthController.GetMaxHP();
            currentValue = healthController.CurrentHealth();
            hpText.text = currentValue + "/" + maxValue;
        }
    }

    public void Setup()
    {
        
        Debug.Log(healthController.transform.parent.name + ": " + healthController + " A " + healthController.CurrentHealth());
        healthController.AddListener(OnHealthChange);

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
