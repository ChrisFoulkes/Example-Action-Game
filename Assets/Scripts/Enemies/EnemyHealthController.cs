using EventCallbacks;

//needs cleaning up heavy overlap with Player HealthController
public class EnemyHealthController : HealthController
{
    private IDamage _damageController;
    private IHeal _healController;


    protected override void Awake()
    {
        base.Awake();
        _damageController = GetComponent<IDamage>();
        _healController = GetComponent<IHeal>();
    }

    // In EnemyHealthController
    public void OnEnable()
    {
        _healController.AddHealListener(OnHealEvent);
        _damageController.AddDamageListener(OnEnemyDamageEvent);
    }

    public void OnDisable()
    {
        _healController.RemoveHealListener(OnHealEvent);
        _damageController.RemoveDamageListener(OnEnemyDamageEvent);
    }

    private void OnEnemyDamageEvent(GameEvent dEvent)
    {
        EnemyDamageEvent damageEvent = (EnemyDamageEvent)dEvent;
        ChangeHealth(damageEvent.DamageInfo.Amount, damageEvent.DamageInfo.IsCriticalHit, damageEvent.DamageInfo.ColourType);
    }

    private void OnHealEvent(GameEvent dEvent)
    {
        EnemyHealEvent healEvent = (EnemyHealEvent)dEvent;
        ChangeHealth(healEvent.HealInfo.Amount, false, FloatingColourType.Heal);
    }
}