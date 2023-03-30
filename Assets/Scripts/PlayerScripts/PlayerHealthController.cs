using EventCallbacks;

//needs cleaning up heavy overlap with Player HealthController
public class PlayerHealthController : HealthController
{
    private IDamage _damageController;
    private IHeal _healController;

    public void Start()
    {
        Initialize(20);
    }
    protected override void GenerateCombatText(float amount, FloatingColourType colourType = FloatingColourType.Generic, bool isCriticalHit = false)
    {
        if (amount > 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText("+" + amount, transform, FloatingColourType.Heal, false);
        }
        else if (amount < 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText(amount.ToString(), transform, colourType, false);
        }
    }
    public void OnEnable()
    {
        _damageController.AddDamageListener(OnDamageEvent);
        _healController.AddHealListener(OnHealEvent);
    }

    public void OnDisable()
    {
        _healController.RemoveHealListener(OnHealEvent);
        _damageController.RemoveDamageListener(OnDamageEvent);
    }

    protected override void Awake()
    {
        base.Awake();
        _damageController = GetComponent<IDamage>();
        _healController = GetComponent<IHeal>();
    }


    private void OnDamageEvent(GameEvent dEvent)
    {
        PlayerDamageEvent damageEvent = (PlayerDamageEvent)dEvent;
        ChangeHealth(damageEvent.DamageInfo.Amount, damageEvent.DamageInfo.IsCriticalHit, damageEvent.DamageInfo.ColourType);
    }
    private void OnHealEvent(GameEvent dEvent)
    {
        PlayerHealEvent healEvent = (PlayerHealEvent)dEvent;
        ChangeHealth(healEvent.HealInfo.Amount, false, FloatingColourType.Heal);
    }
}