using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] private Image amountSprite;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField]  private HealthController healthController;
    private GameObject boss;

    public GameObject Boss
    {
        get { return boss; }
    }

    private void Start()
    {
        if (healthController != null)
        {
            UpdateHealthBar();
        }
    }

    private void OnEnable()
    {
        if (healthController != null)
        {
            healthController.AddListener(OnHealthChange);
        }
    }

    private void OnDisable()
    {
        if (healthController != null)
        {
            healthController.RemoveListener(OnHealthChange);
        }
    }

    public void Initialize(HealthController healthController, GameObject boss)
    {
        this.healthController = healthController;
        this.boss = boss;

        healthController.AddListener(OnHealthChange);
        UpdateHealthBar();
    }

    private void OnHealthChange(GameEvent changeEvent)
    {
        HealthChangedEvent healthEvent = changeEvent as HealthChangedEvent;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float currentValue = healthController.CurrentHealth();
        float maxValue = healthController.GetMaxHP();
        hpText.text = $"{currentValue}/{maxValue}";

        float fillAmount = Mathf.Clamp(currentValue / maxValue, 0, 1);
        Vector3 scale = amountSprite.transform.localScale;
        amountSprite.transform.localScale = new Vector3(fillAmount, scale.y, scale.z);
    }
}
